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
using DocDes.Core.TMFCommon;
using DocDes.Core.Exceptions;

namespace DocDes.Service.Features.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IRepository<Party, int>           _partyRepository;
    private readonly IRepository<ApplicationUser, int> _userRepository;
    private readonly IRepository<RefreshToken, int>    _refreshTokenRepository;
    private readonly ITokenService                     _tokenService;
    private readonly IPasswordHasher                   _passwordHasher;
    private readonly ILogger<RegisterCommandHandler>   _logger;

    public RegisterCommandHandler(
        IRepository<Party, int>           partyRepository,
        IRepository<ApplicationUser, int> userRepository,
        IRepository<RefreshToken, int>    refreshTokenRepository,
        ITokenService                     tokenService,
        IPasswordHasher                   passwordHasher,
        ILogger<RegisterCommandHandler>   logger)
    {
        _partyRepository        = partyRepository;
        _userRepository         = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService           = tokenService;
        _passwordHasher         = passwordHasher;
        _logger                 = logger;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Identifier daha önce alınmış mı?
        var exists = await _userRepository.AnyAsync(
            u => u.DigitalIdentity.Credentials
                .Any(c => c.ContactMedia
                    .Any(cm => cm.Email == request.Identifier
                            || cm.PhoneNumber == request.Identifier)),
            cancellationToken);

        if (exists)
            throw new ConflictException("Bu email veya telefon zaten kayıtlı.");

        // 2. Nesne ağacını kur
        var isEmail = request.Identifier.Contains('@');

        var party = new Party
        {
            PartyType  = PartyType.Individual,
            Individual = new Individual
            {
                GivenName  = request.GivenName,
                FamilyName = request.FamilyName,
                ValidFor   = new TimePeriod { StartDateTime = DateTime.UtcNow }
            },
            PartyRoles = new List<PartyRole>
            {
                new PartyRole
                {
                    PartyRoleTypeCd = "User",
                    ValidFor        = new TimePeriod { StartDateTime = DateTime.UtcNow },
                    DigitalIdentity = new DigitalIdentity
                    {
                        DigitalIdentityDate = DateTime.UtcNow,
                        Credentials = new List<Credential>
                        {
                            new Credential
                            {
                                CredentialType = CredentialType.Password,
                                ContactMedia   = new List<ContactMedium>
                                {
                                    new ContactMedium
                                    {
                                        Email       = isEmail ? request.Identifier : null,
                                        PhoneNumber = isEmail ? null : request.Identifier
                                    }
                                },
                                Characteristics = new List<CredentialCharacteristic>
                                {
                                    new CredentialCharacteristic
                                    {
                                        Name  = "passwordHash",
                                        Value = _passwordHasher.Hash(request.Password)
                                    }
                                }
                            }
                        },
                        ApplicationUser = new ApplicationUser
                        {
                            LanguageId = request.LanguageId,
                        }                        
                    },

                }
            }
        };

        // 3. Tek seferde kaydet — EF navigation üzerinden cascade insert
        await _partyRepository.AddAsync(party, cancellationToken);

        // 4. Kaydedilen ApplicationUser'ı al
        var partyRole   = party.PartyRoles!.First();
        var digitalId   = partyRole.DigitalIdentity
            ?? throw new InvalidOperationException("ApplicationUser oluşturulamadı.");
        var appUser     = digitalId.ApplicationUser
            ?? throw new InvalidOperationException("DigitalIdentity oluşturulamadı.");

        // 5. Token üret
        var (userName, userIdentifier) = ResolveUserInfo(appUser, digitalId, party.Individual!);

        var accessToken  = _tokenService.CreateAccessToken(userName, appUser.Id, userIdentifier);
        var refreshToken = _tokenService.CreateRefreshToken(appUser.Id);

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        _logger.LogInformation(
            "Yeni kullanıcı kaydoldu. UserId: {UserId}, Identifier: {Identifier}",
            appUser.Id, request.Identifier);

        return new AuthResult
        {
            AccessToken            = accessToken.Token,
            AccessTokenExpiration  = accessToken.Expiration,
            RefreshToken           = refreshToken.Token,
            RefreshTokenExpiration = refreshToken.ExpiresAt
        };
    }

    private static (string userName, string userIdentifier) ResolveUserInfo(
        ApplicationUser user,
        DigitalIdentity digitalIdentity,
        Individual individual)
    {
        var userName = $"{individual.GivenName} {individual.FamilyName}".Trim();

        var contactMedium = digitalIdentity.Credentials
            .SelectMany(c => c.ContactMedia)
            .FirstOrDefault();

        var userIdentifier = contactMedium?.Email
            ?? contactMedium?.PhoneNumber
            ?? string.Empty;

        return (userName, userIdentifier);
    }
}