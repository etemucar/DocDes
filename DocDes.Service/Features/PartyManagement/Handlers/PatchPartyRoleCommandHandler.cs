using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Commands;
using DocDes.Core.Repository;
using DocDes.Core.Model;

namespace DocDes.Service.Features.Handlers;

public class PatchPartyRoleCommandHandler : IRequestHandler<PatchPartyRoleCommand, PartyRoleResponse>
{
    private readonly IRepository<PartyRole, int> _partyRoleRepository;
    private readonly ILogger<PatchPartyRoleCommandHandler> _logger;

    public PatchPartyRoleCommandHandler(
        IRepository<PartyRole, int> partyRoleRepository,
        ILogger<PatchPartyRoleCommandHandler> logger)
    {
        _partyRoleRepository = partyRoleRepository;
        _logger              = logger;
    }

    public async Task<PartyRoleResponse> Handle(PatchPartyRoleCommand request, CancellationToken cancellationToken)
    {
        var partyRole = await _partyRoleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (partyRole is null)
            return null!;

        if (request.PartyRoleTypeCd is not null) partyRole.PartyRoleTypeCd = request.PartyRoleTypeCd;

        if (request.ValidFor is not null)
        {
            partyRole.ValidFor.StartDateTime = request.ValidFor.StartDateTime ?? partyRole.ValidFor.StartDateTime;
            partyRole.ValidFor.EndDateTime   = request.ValidFor.EndDateTime   ?? partyRole.ValidFor.EndDateTime;
        }

        await _partyRoleRepository.UpdateAsync(partyRole, cancellationToken);

        _logger.LogInformation("PartyRole güncellendi. PartyRoleId: {PartyRoleId}", partyRole.Id);

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