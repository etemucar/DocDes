using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Queries;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class GetPartyRoleQueryHandler : IRequestHandler<GetPartyRoleQuery, PartyRoleResponse>
{
    private readonly IRepository<PartyRole, int> _partyRoleRepository;
    private readonly ILogger<GetPartyRoleQueryHandler> _logger;

    public GetPartyRoleQueryHandler(
        IRepository<PartyRole, int> partyRoleRepository,
        ILogger<GetPartyRoleQueryHandler> logger)
    {
        _partyRoleRepository = partyRoleRepository;
        _logger              = logger;
    }

    public async Task<PartyRoleResponse> Handle(GetPartyRoleQuery request, CancellationToken cancellationToken)
    {
        var partyRole = await _partyRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (partyRole is null)
            return null!;

        return MapToResponse(partyRole);
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