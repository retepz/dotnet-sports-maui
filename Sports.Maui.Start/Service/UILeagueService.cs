namespace Sports.Maui.Start.Service;

using Sports.Maui.Start.Interface.Service;
using Sports.Maui.Start.Model.UI;
using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;

public class UILeagueService(
    IApiLeagueService apiLeagueService) : IUILeagueService
{
    public async Task<UILeague?> GetCurrentSeason(ApiLeague league)
    {
        var season = await apiLeagueService.GetCurrentSeason(league);
        if (season == null)
        {
            return new(league, season: null, week: null);
        }

        if (season.IsOffSeason)
        {
            return new(league, season: season, week: null);
        }

        var week = await apiLeagueService.GetCurrentWeek(league, season);

        return new(league, season, week);
    }

    public async Task SetMatches(UILeague uiLeague)
    {
        var (newWeek, apiEvents) = await apiLeagueService.GetWeekEvents(uiLeague.ApiLeague, uiLeague.ApiSeason!, uiLeague.ApiWeek);
        if(newWeek != null)
        {
            uiLeague.SetWeek(newWeek);
        }

        if(apiEvents == null)
        {
            return;
        }

        uiLeague.SetMatches(apiEvents);
    }
}
