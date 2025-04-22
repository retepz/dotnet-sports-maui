namespace Sports.Maui.Start.Service;

using Sports.Maui.Service.Interface;
using Sports.Maui.Model;
using Sports.Maui.Start.Interface.Service;
using Sports.Maui.Model.Api;

public class LeaguePreferenceService(IUserPreferenceService userPreferenceService) : ILeaguePreferenceService
{
    private readonly Func<ApiLeague, string> _customTeamsKey = league => $"{UserPreferenceKey.CustomLeague}_{league.Id}";

    public bool? GetCustomTeamsPreference(ApiLeague league)
    {
        var key = _customTeamsKey(league);
        return userPreferenceService.GetBool(key);
    }

    public void SetCustomTeamsPreference(ApiLeague league, bool selected)
    {
        var key = _customTeamsKey(league);
        userPreferenceService.Set(key, selected);
    }

    public void DeleteCustomTeamsPreference(ApiLeague league)
    {
        var key = _customTeamsKey(league);
        userPreferenceService.Delete(key);
    }
}
