using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocDes.Api.Models;
using DocDes.Service.Features.Commands;
using DocDes.Core.Responses;

namespace DocDes.Api.Controllers;

[Authorize]
[Route("api/v1/auth")]
[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly bool _returnTokenInBody;

    public AuthController(IMediator mediator, IConfiguration config)
    {
        _mediator = mediator;
        _returnTokenInBody = config.GetValue<bool>("Auth:ReturnTokenInBody");
    }

    /// <summary>Login</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _mediator.Send(new LoginCommand
        {
            Identifier = model.Identifier,
            Password   = model.Password
        });

        Response.Cookies.Append("auth", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.AccessTokenExpiration,
            Path     = "/"
        });

        Response.Cookies.Append("refresh", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.RefreshTokenExpiration,
            Path     = "/api/v1/auth/refresh"
        });

        if (_returnTokenInBody)
            return Ok(new { success = true, token = result.AccessToken });

        return Ok(new { success = true });
    }

    /// <summary>Refresh access token</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refresh"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new ErrorResponse("Refresh token eksik."));

        var result = await _mediator.Send(new RefreshTokenCommand
        {
            RefreshToken = refreshToken
        });

        Response.Cookies.Append("auth", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.AccessTokenExpiration,
            Path     = "/"
        });

        Response.Cookies.Append("refresh", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.RefreshTokenExpiration,
            Path     = "/api/v1/auth/refresh"
        });

        return Ok(new { success = true });
    }

    /// <summary>Logout</summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refresh"];
        if (!string.IsNullOrEmpty(refreshToken))
            await _mediator.Send(new RevokeTokenCommand { RefreshToken = refreshToken });

        Response.Cookies.Delete("auth");
        Response.Cookies.Delete("refresh");

        return Ok(new { success = true });
    }

    /// <summary>Register</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _mediator.Send(new RegisterCommand
        {
            GivenName  = model.GivenName,
            FamilyName = model.FamilyName,
            Identifier = model.Identifier,
            Password   = model.Password,
            LanguageId = model.LanguageId
        });

        Response.Cookies.Append("auth", result.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.AccessTokenExpiration,
            Path     = "/"
        });

        Response.Cookies.Append("refresh", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure   = true,
            SameSite = SameSiteMode.Strict,
            Expires  = result.RefreshTokenExpiration,
            Path     = "/api/v1/auth/refresh"
        });

        if (_returnTokenInBody)
            return Ok(new { success = true, token = result.AccessToken });

        return Ok(new { success = true });
    }    
}