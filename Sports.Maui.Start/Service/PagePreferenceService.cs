namespace Sports.Maui.Start.Service;

using Sports.Maui.Service.Interface;
using Sports.Maui.Model;
using Sports.Maui.Start.Interface.Service;
using Sports.Maui.Model.Api;

public class PagePreferenceService(IUserPreferenceService userPreferenceService) : IPagePreferenceService
{
    private readonly Func<ApiSport, string> _getLastVisitedLeagueKey = sport => $"{UserPreferenceKey.LastVisitedLeague}_{sport.Id}";

    public string GetLastVisitedLeagueId(ApiSport sport)
    {
        var key = _getLastVisitedLeagueKey(sport);

        return userPreferenceService.GetString(key);
    }

    public bool? GetShowSearchPref()
    {
        return userPreferenceService.GetBool(UserPreferenceKey.ShowSearch.ToString());
    }

    public void SetShowSearchPref(bool newValue)
    {
        userPreferenceService.Set(UserPreferenceKey.ShowSearch.ToString(), newValue);
    }

    public string GetLastVisitedSportId()
    {
        return userPreferenceService.GetString(UserPreferenceKey.LastVisitedSport.ToString());
    }

    public void SetLastVisitedLeagueId(ApiSport sport, ApiLeague league)
    {
        var key = _getLastVisitedLeagueKey(sport);
        userPreferenceService.Set(key, league.Id);
    }

    public void SetLastVisitedSportId(ApiSport sport)
    {
        userPreferenceService.Set(UserPreferenceKey.LastVisitedSport.ToString(), sport.Id);
    }

    public void DeleteLastVisitedLeagueId(ApiSport sport)
    {
        var key = _getLastVisitedLeagueKey(sport);
        userPreferenceService.Delete(key);
    }

    public void DeleteLastVisitedSportId()
    {
        userPreferenceService.Delete(UserPreferenceKey.LastVisitedSport.ToString());
    }
}
