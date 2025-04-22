namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;

public class UISportLeague : TVDpadModel
{
    private LeagueType _leagueType;

    public UISportLeague(ApiLeague apiLeague) 
    {
        LeagueType = apiLeague.LeagueType;
        Name = apiLeague.Name;
        Logo = apiLeague.Logo?.Url;
        ApiLeague = apiLeague;
    }

    public LeagueType LeagueType
    {
        get => _leagueType;
        set
        {
            _leagueType = value;
            NotifyPropertyChanged(nameof(LeagueType));
        }
    }

    public ApiLeague ApiLeague { get; }
}
