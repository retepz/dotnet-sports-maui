namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiLeagueService : IApiCacheItemService<ApiLeague>
{
    Task<ApiSeason?> GetCurrentSeason(ApiLeague league);
    Task<ApiWeek?> GetCurrentWeek(ApiLeague league, ApiSeason season);
    Task<(ApiWeek?, IEnumerable<ApiEvent>?)> GetWeekEvents(ApiLeague league, ApiSeason season, ApiWeek? week);
}
