namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiCompetitor : ApiCacheItem, IApiId
{
    [JsonProperty("team")]
    public ApiUrl TeamUrl { get; set; }

    public string Id { get; set; }

    public bool Winner { get; set; }

    public string HomeAway { get; set; }

    [JsonProperty("score")]
    public ApiUrl ScoreUrl { get; set; }

    [JsonIgnore]
    public ApiTeam CurrentTeam { get; set; }

    [JsonIgnore]
    public ApiScore CurrentScore { get; set; }

    [JsonIgnore]
    public bool IsHome => HomeAway != null && HomeAway.Contains("home", StringComparison.InvariantCultureIgnoreCase);

    public override bool CacheNeverExpires => true;
}
