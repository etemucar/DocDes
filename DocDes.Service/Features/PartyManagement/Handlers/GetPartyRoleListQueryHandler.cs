using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Queries;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class GetPartyRoleListQueryHandler : IRequestHandler<GetPartyRoleListQuery, IEnumerable<PartyRoleResponse>>
{
    private readonly IRepository<PartyRole, int> _partyRoleRepository;
    private readonly ILogger<GetPartyRoleListQueryHandler> _logger;

    public GetPartyRoleListQueryHandler(
        IRepository<PartyRole, int> partyRoleRepository,
        ILogger<GetPartyRoleListQueryHandler> logger)
    {
        _partyRoleRepository = partyRoleRepository;
        _logger              = logger;
    }

    public async Task<IEnumerable<PartyRoleResponse>> Handle(GetPartyRoleListQuery request, CancellationToken cancellationToken)
    {
        var partyRoles = await _partyRoleRepository.FindAsync(
            predicate: _ => true,
            orderBy:   q => q.OrderBy(x => x.Id),
            ct:        cancellationToken);

        return partyRoles.Select(MapToResponse);
    }

    private static PartyRoleResponse MapToResponse(PartyRole partyRole) => new()
    {
        Id              = partyRole.Id,
        PartyId         = partyRole.PartyId,
        PartyRoleTypeCd = partyRole.PartyRoleTypeCd,
        ValidFor = new TimePeriodResponse
        {
            StartDateTime = partyRole.ValidFor.StartDateTime,
            EndDateTime   = partyRole.ValidFor.EndDateTime
        }
    };
}