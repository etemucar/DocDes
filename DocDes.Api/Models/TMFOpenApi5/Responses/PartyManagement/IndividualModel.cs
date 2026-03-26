using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

/// <summary>
/// TMF632 - Individual resource model
/// </summary>
public class IndividualModel : BaseModel
{
    [JsonProperty("givenName")]
    [Required(ErrorMessage = "Ad zorunludur")]
    [MaxLength(100)]
    public string GivenName { get; set; } = null!;

    [JsonProperty("familyName")]
    [Required(ErrorMessage = "Soyad zorunludur")]
    [MaxLength(100)]
    public string FamilyName { get; set; } = null!;

    [JsonProperty("middleName")]
    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [JsonProperty("title")]
    [MaxLength(50)]
    public string? Title { get; set; }

    [JsonProperty("gender")]
    [MaxLength(20)]
    public string? Gender { get; set; }

    [JsonProperty("nationality")]
    [MaxLength(50)]
    public string? Nationality { get; set; }

    [JsonProperty("birthDate")]
    public DateTime? BirthDate { get; set; }

    [JsonProperty("deathDate")]
    public DateTime? DeathDate { get; set; }

    [JsonProperty("placeOfBirth")]
    [MaxLength(100)]
    public string? PlaceOfBirth { get; set; }

    [JsonProperty("countryOfBirth")]
    [MaxLength(100)]
    public string? CountryOfBirth { get; set; }

    [JsonProperty("maritalStatus")]
    [MaxLength(50)]
    public string? MaritalStatus { get; set; }

    [JsonProperty("validFor")]
    public TimePeriodModel? ValidFor { get; set; }

    [JsonProperty("contactMedium")]
    public List<ContactMediumModel> ContactMedium { get; set; } = new();

    [JsonProperty("relatedParty")]
    public List<RelatedPartyModel> RelatedParty { get; set; } = new();
}