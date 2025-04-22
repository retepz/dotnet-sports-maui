namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiWeekService : IApiCacheItemService<ApiWeek>
{
    Task<ApiWeek?> GetCurrentWeek(ApiLeague league, ApiSeason season);
    Task<ApiWeeks?> GetWeeks(ApiUrl apiApiUrl);
}
