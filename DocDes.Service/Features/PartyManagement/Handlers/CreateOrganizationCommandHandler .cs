using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Commands;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;

namespace DocDes.Service.Features.Handlers;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, OrganizationResponse>
{
    private readonly IRepository<Party, int>        _partyRepository;
    private readonly IRepository<Organization, int> _organizationRepository;
    private readonly ILogger<CreateOrganizationCommandHandler> _logger;

    public CreateOrganizationCommandHandler(
        IRepository<Party, int>        partyRepository,
        IRepository<Organization, int> organizationRepository,
        ILogger<CreateOrganizationCommandHandler> logger)
    {
        _partyRepository        = partyRepository;
        _organizationRepository = organizationRepository;
        _logger                 = logger;
    }

    public async Task<OrganizationResponse> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        // 1. Party oluştur (abstract container)
        var party = new Party();
        await _partyRepository.AddAsync(party, cancellationToken);

        // 2. Organization oluştur
        var organization = new Organization
        {
            PartyId             = party.Id,
            Name                = request.Name,
            TaxOffice           = request.TaxOffice,
            TaxNumber           = request.TaxNumber,
            IdentityNumber      = request.IdentityNumber,
            TradeName           = request.TradeName,
            TradeRegisterNumber = request.TradeRegisterNumber,
            MersisNo            = request.MersisNo,
            ValidFor = new TimePeriod
            {
                StartDateTime = request.ValidFor?.StartDateTime ?? DateTime.MinValue,
                EndDateTime   = request.ValidFor?.EndDateTime   ?? DateTime.MaxValue
            }
        };

        await _organizationRepository.AddAsync(organization, cancellationToken);

        // SaveChanges → UnitOfWork pipeline behavior ile çağrılır

        _logger.LogInformation(
            "Organization oluşturuldu. PartyId: {PartyId}, OrganizationId: {OrganizationId}",
            party.Id, organization.Id);

        return MapToResponse(organization);
    }

    private static OrganizationResponse MapToResponse(Organization organization) => new()
    {
        Id                  = organization.Id,
        Name                = organization.Name,
        TaxOffice           = organization.TaxOffice,
        TaxNumber           = organization.TaxNumber,
        IdentityNumber      = organization.IdentityNumber,
        TradeName           = organization.TradeName,
        TradeRegisterNumber = organization.TradeRegisterNumber,
        MersisNo            = organization.MersisNo,
        ValidFor = new TimePeriodResponse
        {
            StartDateTime = organization.ValidFor.StartDateTime,
            EndDateTime   = organization.ValidFor.EndDateTime
        }
    };
}