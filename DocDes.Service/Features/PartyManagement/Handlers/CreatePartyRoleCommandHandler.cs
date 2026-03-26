using MediatR;
using Microsoft.Extensions.Logging;
using DocDes.Service.Dtos.Responses;
using DocDes.Service.Features.Commands;
using DocDes.Core.Repository;
using DocDes.Core.Model;
using DocDes.Core.TMFCommon;

namespace DocDes.Service.Features.Handlers;

public class CreatePartyRoleCommandHandler : IRequestHandler<CreatePartyRoleCommand, PartyRoleResponse>
{
    private readonly IRepository<PartyRole, int> _partyRoleRepository;
    private readonly ILogger<CreatePartyRoleCommandHandler> _logger;

    public CreatePartyRoleCommandHandler(
        IRepository<PartyRole, int> partyRoleRepository,
        ILogger<CreatePartyRoleCommandHandler> logger)
    {
        _partyRoleRepository = partyRoleRepository;
        _logger              = logger;
    }

    public async Task<PartyRoleResponse> Handle(CreatePartyRoleCommand request, CancellationToken cancellationToken)
    {
        var partyRole = new PartyRole
        {
            PartyId         = request.PartyId,
            PartyRoleTypeCd = request.PartyRoleTypeCd,
            ValidFor = new TimePeriod
            {
                StartDateTime = request.ValidFor?.StartDateTime ?? DateTime.MinValue,
                EndDateTime   = request.ValidFor?.EndDateTime   ?? DateTime.MaxValue
            }
        };

        await _partyRoleRepository.AddAsync(partyRole, cancellationToken);

        // SaveChanges → UnitOfWork pipeline behavior ile çağrılır

        _logger.LogInformation(
            "PartyRole oluşturuldu. PartyId: {PartyId}, PartyRoleTypeCd: {PartyRoleTypeCd}, PartyRoleId: {PartyRoleId}",
            partyRole.PartyId, partyRole.PartyRoleTypeCd, partyRole.Id);

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