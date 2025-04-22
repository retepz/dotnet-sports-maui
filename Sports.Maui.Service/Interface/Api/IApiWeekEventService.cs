namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiWeekEventService : IApiCacheItemService<ApiWeekEvent>
{
    Task<(ApiWeek?, ApiWeekEvent)> Get(ApiLeague league, ApiSeason season);
    Task<ApiWeekEvent> Get(ApiLeague league, ApiWeek week);
    ApiUrl BuildEventsUrlFromLeagueUrl(ApiLeague leagueUrl);
}
