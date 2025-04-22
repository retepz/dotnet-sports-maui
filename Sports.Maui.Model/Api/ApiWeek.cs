namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiWeek : ApiCacheItem
{
    public int Number { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [JsonProperty("events")]
    public ApiUrl EventsUrl { get; set; }

    [JsonProperty("text")]
    public string DisplayName { get; set; }
}
