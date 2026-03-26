using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Queries;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class GetOrganizationListQueryHandler : IRequestHandler<GetOrganizationListQuery, IEnumerable<OrganizationResponse>>
{
    private readonly IRepository<Organization, int> _organizationRepository;
    private readonly ILogger<GetOrganizationListQueryHandler> _logger;

    public GetOrganizationListQueryHandler(
        IRepository<Organization, int> organizationRepository,
        ILogger<GetOrganizationListQueryHandler> logger)
    {
        _organizationRepository = organizationRepository;
        _logger                 = logger;
    }

    public async Task<IEnumerable<OrganizationResponse>> Handle(GetOrganizationListQuery request, CancellationToken cancellationToken)
    {
        var organizations = await _organizationRepository.FindAsync(
            predicate: _ => true,
            orderBy:   q => q.OrderBy(x => x.Id),
            ct:        cancellationToken);

        return organizations.Select(MapToResponse);
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