namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiWeeks : ApiCacheItem
{
    [JsonProperty("items")]
    public ApiWeek[] AllWeeks { get; set; }
}
