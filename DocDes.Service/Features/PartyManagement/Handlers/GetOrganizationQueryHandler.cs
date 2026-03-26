using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Queries;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, OrganizationResponse>
{
    private readonly IRepository<Organization, int> _organizationRepository;
    private readonly ILogger<GetOrganizationQueryHandler> _logger;

    public GetOrganizationQueryHandler(
        IRepository<Organization, int> organizationRepository,
        ILogger<GetOrganizationQueryHandler> logger)
    {
        _organizationRepository = organizationRepository;
        _logger                 = logger;
    }

    public async Task<OrganizationResponse> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
    {
        var organization = await _organizationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (organization is null)
            return null!;

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