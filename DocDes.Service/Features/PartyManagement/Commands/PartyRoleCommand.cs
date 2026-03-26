using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class CreatePartyRoleCommand : IRequest<PartyRoleResponse>
{
    public int    PartyId         { get; set; }
    public string PartyRoleTypeCd { get; set; } = null!;
    public TimePeriodRequest? ValidFor { get; set; }
}

public class PatchPartyRoleCommand : IRequest<PartyRoleResponse>
{
    public int     Id              { get; set; }
    public string? PartyRoleTypeCd { get; set; }
    public TimePeriodRequest? ValidFor { get; set; }
}
 
public class DeletePartyRoleCommand : IRequest<bool>
{
    public int Id { get; set; }
}