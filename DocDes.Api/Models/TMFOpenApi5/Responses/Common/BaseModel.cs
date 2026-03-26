using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

public abstract class BaseModel
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; }

    [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
    public string? Href { get; set; }

    [JsonProperty("@baseType", NullValueHandling = NullValueHandling.Ignore)]
    public string? BaseType { get; set; }

    [JsonProperty("@type", NullValueHandling = NullValueHandling.Ignore)]
    public string? Type { get; set; }

    [JsonProperty("@schemaLocation", NullValueHandling = NullValueHandling.Ignore)]
    public string? SchemaLocation { get; set; }
}