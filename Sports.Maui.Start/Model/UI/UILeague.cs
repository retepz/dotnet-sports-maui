namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model.Api;
using Sports.Maui.Model.Helper;

public class UILeague : DpadModel
{
    private string? _headerDisplay;
    private string? _subheadingDisplay;
    private string? _leagueLogo;
    private string? _displayName;

    public UILeague(
        ApiLeague league,
        ApiSeason? season,
        ApiWeek? week)
    {
        ApiLeague = league;
        ApiSeason = season;
        Matches = [];

        var displayNameToUse = league.Abbreviation ?? league.ShortName ?? league.DisplayName;
        var displayNameTo = displayNameToUse.Length > 5 ? 5 : displayNameToUse.Length;
        _displayName = displayNameToUse.Substring(0, displayNameTo);
        _leagueLogo = league.Logo?.Url;

        SetWeek(week);
    }

    public void SetWeek(ApiWeek? week)
    {
        ApiWeek = week;

        if (week != null && week.StartDate.HasValue && week.EndDate.HasValue)
        {
            var startDateLocal = week.StartDate.Value.ToLocalTime();
            var endDateLocal = week.EndDate.Value.ToLocalTime();
            SubheadingDisplay = DateTimeHelper.WeekStartEndDisplay(startDateLocal, endDateLocal);
        }
        else if (ApiSeason != null && !ApiSeason.IsOffSeason && ApiSeason.Type.StartDate.HasValue && ApiSeason.Type.EndDate.HasValue)
        {
            var startDateLocal = ApiSeason.Type.StartDate.Value.ToLocalTime();
            var endDateLocal = ApiSeason.Type.EndDate.Value.ToLocalTime();
            SubheadingDisplay = DateTimeHelper.WeekStartEndDisplay(startDateLocal, endDateLocal);
        }

        HeaderDisplay = GetHeaderDisplay(week, ApiSeason);
    }

    public void SetMatches(IEnumerable<ApiEvent> events) 
    {
        foreach (var apiEvent in events)
        {
            Matches.Add(new(apiEvent));
        }
    }

    public ApiSeason? ApiSeason { get; }
    public ApiLeague ApiLeague { get; }
    public ApiWeek? ApiWeek { get; private set; }

    public string HeaderDisplay
    {
        get => _headerDisplay;
        set
        {
            _headerDisplay = value;
            NotifyPropertyChanged(nameof(HeaderDisplay));
        }
    }

    public bool ShowHeaderDisplay => !string.IsNullOrEmpty(HeaderDisplay);

    public string SubheadingDisplay
    {
        get => _subheadingDisplay;
        set
        {
            _subheadingDisplay = value;
            NotifyPropertyChanged(nameof(SubheadingDisplay));
        }
    }

    public string LeagueLogo
    {
        get => _leagueLogo;
        set
        {
            _leagueLogo = value;
            NotifyPropertyChanged(nameof(LeagueLogo));
        }
    }

    public string DisplayName
    {
        get => _displayName;
        set
        {
            _displayName = value;
            NotifyPropertyChanged(nameof(DisplayName));
        }
    }

    public IList<UIMatch> Matches { get; set; }

    private string? GetHeaderDisplay(ApiWeek? week, ApiSeason? season)
    {
        if (week?.DisplayName != null)
        {
            return week.DisplayName;
        }

        if(season == null || season.IsOffSeason)
        {
            return null;
        }

        return season?.Type.Name ?? season?.DisplayName;
    }
}
