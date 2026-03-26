using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

public class PartyOrPartyRoleModel
{
    [JsonProperty("id")]
    public string Id { get; set; } = null!;

    [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
    public string Href { get; set; } = null!;

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; } = null!;

    [JsonProperty("role")]
    public string Role { get; set; } = null!;

    [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; } = "RelatedParty";

    [JsonProperty("@referredType", NullValueHandling = NullValueHandling.Ignore)]
    public string ReferredType { get; set; } = null!;
}

public class RelatedPartyModel
{
    [JsonProperty("role", Required = Required.Always)]
    public string Role { get; set; } = null!;

    [JsonProperty("@type")]
    public string Type { get; set; } = "RelatedPartyRefOrPartyRoleRef";

    [JsonProperty("partyOrPartyRole")]
    public PartyOrPartyRoleModel PartyOrPartyRole { get; set; } = new();
}    