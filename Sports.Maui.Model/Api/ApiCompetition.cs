namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiCompetition : ApiCacheItem, IApiId
{
    [JsonProperty("competitors")]
    public ApiUrl[] CompetitorUrls { get; set; }

    public string Id { get; set; }

    [JsonProperty("status")]
    public ApiUrl StatusUrl { get; set; }

    [JsonProperty("broadcasts")]
    public ApiUrl BroadcastUrl { get; set; }

    [JsonProperty("Situation")]
    public ApiUrl SituationUrl { get; set; }

    public DateTime Date { get; set; }
    public int Attendance { get; set; }

    [JsonIgnore]
    public ApiStatus CurrentStatus { get; set; }

    [JsonIgnore]
    public ApiBroadcast CurrentBroadcast { get; set; }

    [JsonIgnore]
    public ApiSituation CurrentSituation { get; set; }

    [JsonIgnore]
    public IEnumerable<ApiCompetitor> CurrentCompetitors { get; set; }

    [JsonProperty("type")]
    public ApiCompetitionType CurrentType { get; set; }

    [JsonIgnore]
    public ApiCompetitor? CompetitorWithPosession
    {
        get
        {
            var gameInProgress = CurrentStatus?.IsInProgress ?? false;
            if (FirstCompetitor == null || CurrentSituation?.CurrentTeamPossessionUrl?.Url == null || !gameInProgress)
            {
                return null;
            }

            var firstTeamUrl = new Uri(FirstCompetitor.TeamUrl.Url).AbsolutePath;
            var currentSituationUrl = new Uri(CurrentSituation.CurrentTeamPossessionUrl.Url).AbsolutePath;
            var isFirstCompetitor = firstTeamUrl == currentSituationUrl;

            return isFirstCompetitor ? FirstCompetitor : SecondCompetitior;
        }
    }


    [JsonIgnore]
    public ApiCompetitor FirstCompetitor => CurrentCompetitors.First(c => c.IsHome);

    [JsonIgnore]
    public ApiCompetitor SecondCompetitior => CurrentCompetitors.First(c => !c.IsHome);

    public override bool CacheNeverExpires => true;

}
