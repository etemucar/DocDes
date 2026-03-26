using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;


public class RegisterCommand : IRequest<AuthResult>
{
    public string GivenName    { get; set; } = null!;
    public string FamilyName   { get; set; } = null!;
    public string Identifier   { get; set; } = null!; // email veya telefon
    public string Password     { get; set; } = null!;
    public int    LanguageId   { get; set; }
}