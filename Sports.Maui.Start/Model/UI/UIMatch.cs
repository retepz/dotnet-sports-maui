namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model.Api;
using Sports.Maui.Model.Helper;

public class UIMatch : DpadModel
{
    private readonly ApiEvent _apiEvent;

    private readonly UIMatchTeam _firstTeam;
    private readonly UIMatchTeam _secondTeam;
    private readonly string _broadcastStations;

    public ApiCompetition ApiCompetition => _apiEvent?.Competition!;

    public UIMatch(
        ApiEvent apiEvent)
    {
        _apiEvent = apiEvent;
        var apiCompetition = apiEvent?.Competition!;

        ApiCompetitor firstApiCompetitor = apiCompetition?.FirstCompetitor!;
        ApiCompetitor secondApiCompetitor = apiCompetition?.SecondCompetitior!;

        _firstTeam = new(apiCompetition, firstApiCompetitor, apiEvent.LeagueType);
        _secondTeam = new(apiCompetition, secondApiCompetitor, apiEvent.LeagueType);

        if (apiCompetition?.CurrentBroadcast == null)
        {
            return;
        }

        _broadcastStations = string.Join(", ", apiCompetition?.CurrentBroadcast.Items.Select(item => item?.Media?.ShortName ?? item.Station));
    }

    public string Url { get; private set; }

    public UIMatchTeam FirstTeam
    {
        get => _firstTeam;
    }

    public UIMatchTeam SecondTeam
    {
        get => _secondTeam;
    }

    public DateTime? GameTime
    {
        get => ApiCompetition?.Date.ToLocalTime();
    }

    public string QuarterDisplay => (IsLive
        ? ApiCompetition.CurrentStatus.StatusType.ShortDetail
        : DateTimeHelper.GetGameDateDisplay(GameTime)) ?? string.Empty;

    public string GameTimeDisplay => (IsInFuture
        ? DateTimeHelper.GameTimeDisplay(GameTime)
        : IsLive
        ? ApiCompetition.CurrentSituation?.DownDistanceText
        : "Final") ?? string.Empty;

    public bool IsFinished
    {
        get => ApiCompetition?.CurrentStatus?.IsCompleted ?? false;
    }

    public bool IsInFuture
    {
        get => ApiCompetition?.CurrentStatus?.IsInFuture ?? false;
    }

    public bool IsLive
    {
        get => ApiCompetition?.CurrentStatus?.IsInProgress ?? false;
    }

    public string BroadcastStations
    {
        get => _broadcastStations;
    }

    public string MatchId => _apiEvent.Id;

    public override Color DefaultColor => Colors.Transparent;
}
