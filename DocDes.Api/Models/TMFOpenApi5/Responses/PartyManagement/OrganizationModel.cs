using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

/// <summary>
/// TMF632 - Organization resource model
/// </summary>
public class OrganizationModel : BaseModel
{
    [JsonProperty("name")]
    [Required(ErrorMessage = "Şirket adı zorunludur")]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [JsonProperty("taxOffice")]
    [MaxLength(100)]
    public string? TaxOffice { get; set; }

    [JsonProperty("taxNumber")]
    public long TaxNumber { get; set; }

    [JsonProperty("identityNumber")]
    public long IdentityNumber { get; set; }

    [JsonProperty("tradeName")]
    [MaxLength(200)]
    public string? TradeName { get; set; }

    [JsonProperty("tradeRegisterNumber")]
    public long TradeRegisterNumber { get; set; }

    [JsonProperty("mersisNo")]
    public long MersisNo { get; set; }

    [JsonProperty("validFor")]
    public TimePeriodModel? ValidFor { get; set; }

    [JsonProperty("contactMedium")]
    public List<ContactMediumModel> ContactMedium { get; set; } = new();

    [JsonProperty("relatedParty")]
    public List<RelatedPartyModel> RelatedParty { get; set; } = new();
}