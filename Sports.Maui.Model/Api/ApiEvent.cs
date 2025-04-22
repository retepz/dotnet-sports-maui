namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiEvent : ApiCacheItem, IApiId
{
    [JsonProperty("competitions")]
    public ApiUrl[] CompetitionUrls { get; set; }

    public string Id { get; set; }

    [JsonIgnore]
    public IEnumerable<ApiCompetition> CurrentCompetitions { get; set; }

    public LeagueType LeagueType { get; set; }

    [JsonIgnore]
    public ApiCompetition Competition => CurrentCompetitions.First();

    [JsonProperty("week")]
    public ApiUrl WeekUrl { get; set; }
}
