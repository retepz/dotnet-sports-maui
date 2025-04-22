namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;

public class UIMatchTeam : NotifyPropertyChangedModel
{
    private string _name;
    private string _location;
    public string _logo;
    public string _score;
    private string _homeAway;
    public bool _hasLogo;
    public bool _hasPossession;
    public string _record;
    public Color _color;

    public UIMatchTeam(
        ApiCompetition apiCompetition,
        ApiCompetitor apiCompetitor,
        LeagueType leagueType)
    {
        _location = apiCompetitor?.CurrentTeam?.Location ?? string.Empty;
        _name = GetName(apiCompetitor, _location, leagueType);
        _logo = GetLogo(apiCompetitor);
        _score = GetScore(apiCompetition, apiCompetitor);
        _homeAway = GetHomeAway(apiCompetitor);
        _hasPossession = GetHasPossession(apiCompetition, apiCompetitor);
        _record = apiCompetitor?.CurrentTeam?.CurrentRecord?.DisplayValue;
        _color = Color.FromArgb($"#{apiCompetitor?.CurrentTeam?.Color}");
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NotifyPropertyChanged(nameof(Name));
        }
    }

    public string Location
    {
        get => _location;
        set
        {
            _location = value;
            NotifyPropertyChanged(nameof(Location));
        }
    }

    public string Logo
    {
        get => _logo;
        set
        {
            _logo = value;
            NotifyPropertyChanged(nameof(Logo));
            NotifyPropertyChanged(nameof(HasLogo));
        }
    }

    public string Score
    {
        get => _score;
        set
        {
            _score = value;
            NotifyPropertyChanged(nameof(Score));
        }
    }

    public string HomeAway
    {
        get => _homeAway;
        set
        {
            _homeAway = value;
            NotifyPropertyChanged(nameof(HomeAway));
        }
    }

    public bool HasLogo
    {
        get => !string.IsNullOrEmpty(_logo);
    }

    public bool HasPossession
    {
        get => _hasPossession;
        set
        {
            _hasPossession = value;
            NotifyPropertyChanged(nameof(HasPossession));
        }
    }

    public string Record
    {
        get => _record;
        set
        {
            _record = value;
            NotifyPropertyChanged(nameof(Record));
        }
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            NotifyPropertyChanged(nameof(Color));
        }
    }

    private static string GetName(ApiCompetitor apiCompetitor,
        string location,
        LeagueType leagueType)
    {
        var apiName = apiCompetitor?.CurrentTeam?.Name;
        var apiShortName = apiCompetitor?.CurrentTeam?.ShortDisplayName;

        if(leagueType == LeagueType.CollegeFootball)
        {
            return apiName ?? apiShortName ?? string.Empty;
        }

        var shortNameIsLocation = !string.IsNullOrEmpty(location)
            && !string.IsNullOrEmpty(apiShortName)
            && location.Contains(apiShortName, StringComparison.InvariantCultureIgnoreCase);

        if (!string.IsNullOrEmpty(apiName) && shortNameIsLocation)
        {
            return apiName;
        }
        return apiShortName ?? apiName ?? string.Empty;
    }

    private static string GetLogo(ApiCompetitor apiCompetitor)
    {
        return apiCompetitor?.CurrentTeam?.ScoreboardLogo?.Url ?? apiCompetitor?.CurrentTeam?.DefaultLogo?.Url ?? string.Empty;
    }

    private static string GetScore(
        ApiCompetition apiCompetition,
        ApiCompetitor apiCompetitor)
    {
        if (apiCompetition != null && apiCompetitor != null)
        {
            var apiCompInFuture = apiCompetition != null && apiCompetition.CurrentStatus.IsInFuture;
            if (apiCompInFuture) 
            {
                return string.Empty;
            }
            return apiCompetitor?.CurrentScore?.DisplayValue ?? string.Empty;
        }

        return string.Empty;
    }

    private static string GetHomeAway(ApiCompetitor apiCompetitor)
    {
        if (apiCompetitor != null)
        {
            return apiCompetitor.IsHome ? "Home" : "Away";
        }

        return string.Empty;
    }

    private static bool GetHasPossession(ApiCompetition apiCompetition, ApiCompetitor apiCompetitor)
    {
        if (apiCompetition?.CompetitorWithPosession == null || apiCompetitor == null)
        {
            return false;
        }

        return apiCompetition.CompetitorWithPosession.Id == apiCompetitor.Id;
    }
}
