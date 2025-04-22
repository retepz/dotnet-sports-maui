namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiWeekEvent : ApiEventCollection
{
    [JsonProperty("$meta")]
    public ApiWeekEventMeta Meta { get; set; }
}