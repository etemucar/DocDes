using System.ComponentModel.DataAnnotations;
using DocDes.Core.Enums;
using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

public class DigitalIdentityModel
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("href")]
    public string? Href { get; set; }

    [JsonProperty("nickname")]
    public string? Nickname { get; set; }

    [Required]
    [JsonProperty("partyRoleId")]
    public int PartyRoleId { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("credential")]
    public List<CredentialModel> Credentials { get; set; } = new();
}

public class CredentialModel
{
    [Required]
    [JsonProperty("credentialType")]
    public CredentialType CredentialType { get; set; }

    [JsonProperty("trustLevel")]
    public int? TrustLevel { get; set; }

    [JsonProperty("characteristic")]
    public List<CredentialCharacteristicModel> Characteristics { get; set; } = new();

    [JsonProperty("contactMedium")]
    public List<ContactMediumModel> ContactMedia { get; set; } = new();
}

public class CredentialCharacteristicModel
{
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [Required]
    [JsonProperty("value")]
    public string Value { get; set; } = null!;
}