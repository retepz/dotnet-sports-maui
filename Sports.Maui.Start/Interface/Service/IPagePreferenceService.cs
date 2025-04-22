namespace Sports.Maui.Start.Interface.Service;

using Sports.Maui.Model.Api;

public interface IPagePreferenceService
{
    string GetLastVisitedLeagueId(ApiSport sport);
    string GetLastVisitedSportId();

    void SetLastVisitedLeagueId(ApiSport sport, ApiLeague league);

    void SetLastVisitedSportId(ApiSport sport);

    void DeleteLastVisitedLeagueId(ApiSport sport);

    void DeleteLastVisitedSportId();
    bool? GetShowSearchPref();
    void SetShowSearchPref(bool newValue);
}
