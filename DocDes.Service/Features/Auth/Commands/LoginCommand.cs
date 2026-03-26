using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class LoginCommand : IRequest<AuthResult>
{
    public string Identifier { get; set; } = null!;
    public string Password { get; set; } = null!;
}