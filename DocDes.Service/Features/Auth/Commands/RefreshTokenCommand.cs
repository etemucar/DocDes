using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class RefreshTokenCommand : IRequest<AuthResult>
{
    public string RefreshToken { get; set; } = null!;
}