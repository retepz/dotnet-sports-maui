namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiSituation : ApiCacheItem
{
    public string DownDistanceText { get; set; }
    public int HomeTimeouts { get; set; }
    public int AwayTimeouts { get; set; }

    [JsonProperty("team")]
    public ApiUrl CurrentTeamPossessionUrl { get; set; }
}
