using MediatR;
using DocDes.Service.Dtos.Requests;
using DocDes.Service.Dtos.Responses;

namespace DocDes.Service.Features.Commands;

public class CreateOrganizationCommand : IRequest<OrganizationResponse>
{
    public string  Name                { get; set; } = null!;
    public string? TaxOffice           { get; set; }
    public long    TaxNumber           { get; set; }
    public long    IdentityNumber      { get; set; }
    public string? TradeName           { get; set; }
    public long    TradeRegisterNumber { get; set; }
    public long    MersisNo            { get; set; }
    public TimePeriodRequest?          ValidFor      { get; set; }
    public List<ContactMediumRequest>  ContactMedium { get; set; } = new();
    public List<RelatedPartyRequest>   RelatedParty  { get; set; } = new();
}

public class PatchOrganizationCommand : IRequest<OrganizationResponse>
{
    public int     Id                   { get; set; }
    public string? Name                 { get; set; }
    public string? TaxOffice            { get; set; }
    public long?   TaxNumber            { get; set; }
    public long?   IdentityNumber       { get; set; }
    public string? TradeName            { get; set; }
    public long?   TradeRegisterNumber  { get; set; }
    public long?   MersisNo             { get; set; }
    public TimePeriodRequest? ValidFor  { get; set; }
}
 
public class DeleteOrganizationCommand : IRequest<bool>
{
    public int Id { get; set; }
}