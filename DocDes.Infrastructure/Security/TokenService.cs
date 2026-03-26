using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DocDes.Core.Model;
using DocDes.Core.Security;
using DocDes.Settings.Core;


namespace DocDes.Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly TokenOptions _tokenOptions;

    public TokenService(IOptions<AppSettings> options)
    {
        _tokenOptions = options.Value.TokenOptions;
    }

    public AccessToken CreateAccessToken(ApplicationUser user)
    {
        var credential = user.DigitalIdentity?.Credentials
            .SelectMany(c => c.ContactMedia)
            .FirstOrDefault();

        var userCredential = credential?.Email 
            ?? credential?.PhoneNumber 
            ?? string.Empty;

        return CreateAccessToken(
            userName: user.DigitalIdentity?.Nickname ?? string.Empty,
            userId: user.Id,
            userCredential: userCredential
        );
    }

    public AccessToken CreateAccessToken(string userName, int userId, string userCredential)
    {
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            Expires = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Sid, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userCredential),
                new Claim(ClaimTypes.Name, userName)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return new AccessToken
        {
            User = new TokenUser (userName, userId, userCredential),
            Token = token,
            Expiration = tokenDescriptor.Expires.Value
        };
    }

    public RefreshToken CreateRefreshToken(int userId)
    {
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenOptions.Issuer,
            Audience = _tokenOptions.Audience,
            Expires = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenExpiration), // 30 gün
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Sid, userId.ToString()),
                new Claim("token_type", "refresh")
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return new RefreshToken
        {
            ApplicationUserId = userId,
            Token             = token,
            ExpiresAt         = tokenDescriptor.Expires!.Value,
            CreatedAt         = DateTime.UtcNow,
            IsRevoked         = false
        };
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenOptions.Issuer,
                ValidAudience = _tokenOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }    
} 