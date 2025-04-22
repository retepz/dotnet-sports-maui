namespace Sports.Maui.Start.Interface.Service;

using Sports.Maui.Model.Api;

public interface ILeaguePreferenceService
{
    bool? GetCustomTeamsPreference(ApiLeague league);

    void SetCustomTeamsPreference(ApiLeague league, bool selected);

    void DeleteCustomTeamsPreference(ApiLeague league);
}
