using Newtonsoft.Json;

namespace DocDes.Api.Models.TMFOpenApi5;

public class TimePeriodModel
{
    [JsonProperty("startDateTime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? StartDateTime { get; set; }

    [JsonProperty("endDateTime", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? EndDateTime { get; set; }
}