namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiSeasonType : ApiCacheItem
{
    [JsonProperty("week")]
    public ApiWeek? CurrentWeek { get; set; }

    [JsonProperty("weeks")]
    public ApiWeek? CurrentWeeks { get; set; }

    [JsonProperty("type")]
    public ApiSeasonTypeId TypeId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("startDate")]
    public DateTime? StartDate { get; set; }

    [JsonProperty("endDate")]
    public DateTime? EndDate { get; set; }
}
