using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

/// <summary>
/// TMF632 - PartyRole resource model
/// </summary>
public class PartyRoleModel : BaseModel
{
    [JsonProperty("partyId")]
    [Required(ErrorMessage = "Party ID zorunludur")]
    public int PartyId { get; set; }

    [JsonProperty("partyRoleTypeCd")]
    [Required(ErrorMessage = "Party role tipi zorunludur")]
    [MaxLength(100)]
    public string PartyRoleTypeCd { get; set; } = null!;

    [JsonProperty("validFor")]
    public TimePeriodModel? ValidFor { get; set; }
}