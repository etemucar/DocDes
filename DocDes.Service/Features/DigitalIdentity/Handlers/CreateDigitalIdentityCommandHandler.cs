using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.Enums;
using DocDes.Core.Security;
using DocDes.Service.Features.Commands;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Dtos.Requests;

namespace DocDes.Service.Features.Handlers;

public class CreateDigitalIdentityCommandHandler
    : IRequestHandler<CreateDigitalIdentityCommand, DigitalIdentityResponse>
{
    private readonly IRepository<DigitalIdentity, Guid>  _digitalIdentityRepository;
    private readonly IRepository<ApplicationUser, int>   _userRepository;
    private readonly IRepository<PartyRole, int>         _partyRoleRepository;
    private readonly IPasswordHasher                     _passwordHasher;
    private readonly ILogger<CreateDigitalIdentityCommandHandler> _logger;

    public CreateDigitalIdentityCommandHandler(
        IRepository<DigitalIdentity, Guid>               digitalIdentityRepository,
        IRepository<ApplicationUser, int>                userRepository,
        IRepository<PartyRole, int>                      partyRoleRepository,
        IPasswordHasher                                  passwordHasher,
        ILogger<CreateDigitalIdentityCommandHandler>     logger)
    {
        _digitalIdentityRepository = digitalIdentityRepository;
        _userRepository            = userRepository;
        _partyRoleRepository       = partyRoleRepository;
        _passwordHasher            = passwordHasher;
        _logger                    = logger;
    }

    public async Task<DigitalIdentityResponse> Handle(
        CreateDigitalIdentityCommand request,
        CancellationToken cancellationToken)
    {
        // 1. PartyRole var mı kontrol et
        var partyRole = await _partyRoleRepository.FindOneAsync(
            pr => pr.Id == request.PartyRoleId,
            ct: cancellationToken);

        if (partyRole is null)
            throw new KeyNotFoundException($"PartyRole bulunamadı. Id: {request.PartyRoleId}");

        // 2. Credential'ları oluştur
        var credentials = request.Credentials.Select(cr => new Credential
        {
            CredentialType = cr.CredentialType,
            TrustLevel     = cr.TrustLevel,
            Characteristics = cr.Characteristics.Select(ch => new CredentialCharacteristic
            {
                Name  = ch.Name == "password" ? "passwordHash" : ch.Name,
                Value = ch.Name == "password"
                    ? _passwordHasher.Hash(ch.Value)
                    : ch.Value
            }).ToList(),
            ContactMedia = cr.ContactMedia.Select(cm => MapContactMedium(cm, partyRole.PartyId)).ToList()
        }).ToList();

        // 3. DigitalIdentity oluştur
        var digitalIdentity = new DigitalIdentity
        {
            Nickname            = request.Nickname,
            Status              = GeneralStatus.Active,
            DigitalIdentityDate = DateTime.UtcNow,
            PartyRoleId         = request.PartyRoleId,
            Credentials         = credentials
        };

        await _digitalIdentityRepository.AddAsync(digitalIdentity, cancellationToken);

        // 4. ApplicationUser oluştur
        var applicationUser = new ApplicationUser
        {
            DigitalIdentityId = digitalIdentity.Id
        };

        await _userRepository.AddAsync(applicationUser, cancellationToken);

        _logger.LogInformation(
            "DigitalIdentity oluşturuldu. Id: {Id}, PartyRoleId: {PartyRoleId}",
            digitalIdentity.Id, request.PartyRoleId);

        return MapToResponse(digitalIdentity);
    }

    private static ContactMedium MapContactMedium(ContactMediumRequest cm, int partyId)
    {
        var mediumType = Enum.Parse<ContactMediumType>(cm.MediumType, ignoreCase: true);

        string? email       = null;
        string? phoneNumber = null;

        switch (mediumType)
        {
            case ContactMediumType.EmailAddress:
                email = cm.Characteristic.GetValueOrDefault("emailAddress")?.ToString();
                break;
            case ContactMediumType.PhoneNumber:
                phoneNumber = cm.Characteristic.GetValueOrDefault("phoneNumber")?.ToString();
                break;
        }

        return new ContactMedium
        {
            PartyId     = partyId,
            MediumType  = mediumType,
            IsPreferred = cm.Preferred,
            Email       = email,
            PhoneNumber = phoneNumber
        };
    }

    private static DigitalIdentityResponse MapToResponse(DigitalIdentity d) => new()
    {
        Id                  = d.Id,
        Nickname            = d.Nickname,
        Status              = d.Status,
        DigitalIdentityDate = d.DigitalIdentityDate,
        PartyRoleId         = d.PartyRoleId
    };
}