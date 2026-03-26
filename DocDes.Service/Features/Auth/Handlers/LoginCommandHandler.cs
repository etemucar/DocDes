using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.Security;
using DocDes.Service.Features.Commands;
using DocDes.Service.Dtos.Responses;
using DocDes.Core.Enums;
using DocDes.Core.Exceptions;

namespace DocDes.Service.Features.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IRepository<ApplicationUser, int> _userRepository;
    private readonly IRepository<RefreshToken, int>    _refreshTokenRepository;
    private readonly ITokenService                     _tokenService;
    private readonly IPasswordHasher                   _passwordHasher;
    private readonly ILogger<LoginCommandHandler>      _logger;

    public LoginCommandHandler(
        IRepository<ApplicationUser, int> userRepository,
        IRepository<RefreshToken, int>    refreshTokenRepository,
        ITokenService                     tokenService,
        IPasswordHasher                   passwordHasher,
        ILogger<LoginCommandHandler>      logger)
    {
        _userRepository         = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService           = tokenService;
        _passwordHasher         = passwordHasher;
        _logger                 = logger;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Identifier ile kullanıcıyı bul (email veya telefon)
        var user = await _userRepository.FindOneAsync(
            u => u.DigitalIdentity.Credentials
                .Any(c => c.ContactMedia
                    .Any(cm => cm.Email == request.Identifier
                            || cm.PhoneNumber == request.Identifier)),
            i => i
                .Include(u => u.DigitalIdentity)
                    .ThenInclude(d => d.Credentials)
                        .ThenInclude(c => c.ContactMedia)
                .Include(u => u.DigitalIdentity)
                    .ThenInclude(d => d.Credentials)
                        .ThenInclude(c => c.Characteristics)
                .Include(u => u.DigitalIdentity)
                    .ThenInclude(u => u.PartyRole)
                        .ThenInclude(pr => pr.Party)
                            .ThenInclude(p => p.Individual),
            asNoTracking: false,
            ct: cancellationToken);

        if (user is null)
            throw new UnauthorizedException("Kullanıcı bulunamadı.");

        // 2. DigitalIdentity status kontrolü
        if (user.DigitalIdentity.Status != GeneralStatus.Active)
            throw new UnauthorizedException("Hesap aktif değil.");

        // 3. Şifre doğrula
        var passwordCredential = user.DigitalIdentity.Credentials
            .FirstOrDefault(c => c.CredentialType == CredentialType.Password);

        if (passwordCredential is null)
            throw new UnauthorizedException("Geçersiz kimlik bilgisi.");

        var passwordHash = passwordCredential.Characteristics
            .FirstOrDefault(ch => ch.Name == "passwordHash")?.Value;

        if (!_passwordHasher.Verify(request.Password, passwordHash))
            throw new UnauthorizedException("Kullanıcı adı veya şifre hatalı.");

        // 4. Kullanıcı bilgilerini hazırla
        var (userName, userIdentifier) = ResolveUserInfo(user);

        // 5. Access token üret
        var accessToken = _tokenService.CreateAccessToken(userName, user.Id, userIdentifier);

        // 6. Refresh token üret ve kaydet
        var refreshToken = _tokenService.CreateRefreshToken(user.Id);
        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            ApplicationUserId = user.Id,
            Token             = refreshToken.Token,
            ExpiresAt         = refreshToken.ExpiresAt,
            CreatedAt         = DateTime.UtcNow,
            CreatedByIp       = null
        }, cancellationToken);

        _logger.LogInformation(
            "Kullanıcı giriş yaptı. UserId: {UserId}, Identifier: {Identifier}",
            user.Id, request.Identifier);

        return new AuthResult
        {
            AccessToken            = accessToken.Token,
            AccessTokenExpiration  = accessToken.Expiration,
            RefreshToken           = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresAt
        };
    }

    private static (string userName, string userIdentifier) ResolveUserInfo(ApplicationUser user)
    {
        var individual  = user.DigitalIdentity.PartyRole?.Party?.Individual;

        var userName = individual != null
            ? $"{individual.GivenName} {individual.FamilyName}".Trim()
            : user.DigitalIdentity.Nickname ?? string.Empty;

        var contactMedium = user.DigitalIdentity.Credentials
            .SelectMany(c => c.ContactMedia)
            .FirstOrDefault();

        var userIdentifier = contactMedium?.Email
            ?? contactMedium?.PhoneNumber
            ?? string.Empty;

        return (userName, userIdentifier);
    }
}