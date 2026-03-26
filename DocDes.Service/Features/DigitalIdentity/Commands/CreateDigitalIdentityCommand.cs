using MediatR;
using DocDes.Core.Enums;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class CreateDigitalIdentityCommand : IRequest<DigitalIdentityResponse>
{
    public string? Nickname { get; set; }
    public int PartyRoleId { get; set; }
    public List<CredentialRequest> Credentials { get; set; } = new();
}