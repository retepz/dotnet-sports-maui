namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiWeekService(
    ICacheService cacheService,
    IApiService<ApiWeek> apiService,
    IApiCacheItemService<ApiWeeks> apiWeeksService) 
    : ApiCacheItemService<ApiWeek>(cacheService, apiService), IApiWeekService
{
    private readonly LeagueType[] _useCurrentWeeksLeagues = [LeagueType.CollegeFootball];
    public async Task<ApiWeek?> GetCurrentWeek(ApiLeague league, ApiSeason season)
    {
        if (season.CurrentWeekUrl != null)
        {
            return await Get(season.CurrentWeekUrl);
        }

        if (season.CurrentWeeksUrl == null || !_useCurrentWeeksLeagues.Contains(league.LeagueType))
        {
            return null;
        }

        var result = await GetWeeks(season.CurrentWeeksUrl);
        if (result == null || result.AllWeeks == null || result.AllWeeks.Length == 0)
        {
            return null;
        }

        var lastWeek = result.AllWeeks[result.AllWeeks.Length - 1];
        return lastWeek;
    }

    public async Task<ApiWeeks?> GetWeeks(ApiUrl apiApiUrl)
    {
        return await apiWeeksService.Get(apiApiUrl);
    }
}
