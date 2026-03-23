using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class CreateIndividualCommand : IRequest<IndividualResponse>
{
    public string GivenName      { get; set; } = null!;
    public string FamilyName     { get; set; } = null!;
    public string? MiddleName     { get; set; }
    public string? Title          { get; set; }
    public string? Gender         { get; set; }
    public string? Nationality    { get; set; }
    public DateTime? BirthDate   { get; set; }
    public string? PlaceOfBirth   { get; set; }
    public string? CountryOfBirth { get; set; }
    public string? MaritalStatus  { get; set; }
    public TimePeriodRequest?          ValidFor      { get; set; }
    public List<ContactMediumRequest> ContactMedium { get; set; } = new();
    public List<RelatedPartyRequest>  RelatedParty  { get; set; } = new();
}