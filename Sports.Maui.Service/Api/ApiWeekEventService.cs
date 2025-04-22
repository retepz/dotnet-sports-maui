namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiWeekEventService(
    ICacheService cacheService,
    IApiService<ApiWeekEvent> apiService,
    IApiWeekService weekService)
    : ApiCacheItemService<ApiWeekEvent>(cacheService, apiService), IApiWeekEventService
{

    public async Task<(ApiWeek?, ApiWeekEvent)> Get(ApiLeague league, ApiSeason season)
    {
        var weekEventUrl = BuildEventsUrlFromLeagueUrl(league);
        var apiWeekEvent = await GetUpdatedEvents(weekEventUrl, league);
        if(season.CurrentWeeksUrl == null)
        {
            return (null, apiWeekEvent);
        }

        if (apiWeekEvent.Meta?.Parameters?.Week == null || apiWeekEvent.Meta?.Parameters?.Week.Length != 1)
        {
            return (null, apiWeekEvent);
        }

        var seasonWeekUrls = await weekService.GetWeeks(season.CurrentWeeksUrl);
        if(seasonWeekUrls == null || seasonWeekUrls.AllWeeks.Length == 0)
        {
            return (null, apiWeekEvent);
        }

        var weekNumberFromMeta = int.Parse(apiWeekEvent.Meta.Parameters.Week[0]);
        foreach(var seasonWeekUrl in seasonWeekUrls.AllWeeks)
        {
            var week = await weekService.Get(seasonWeekUrl);
            if(week == null || week.Number != weekNumberFromMeta)
            {
                continue;
            }

            return (week, apiWeekEvent);
        }

        return (null, apiWeekEvent);
    }

    public async Task<ApiWeekEvent> Get(ApiLeague league, ApiWeek week)
    {
        var useLeagueUrl = new LeagueType[]
        {
            LeagueType.CollegeBasketball,
            LeagueType.NFL,
            LeagueType.NBAGLeague,
            LeagueType.NBA
        };

        if (useLeagueUrl.Contains(league.LeagueType))
        {
            var tempWeek = new ApiWeek()
            {
                EventsUrl = BuildEventsUrlFromLeagueUrl(league),
            };

            return await GetUpdatedEvents(tempWeek.EventsUrl, league);
        }

        if (week.EventsUrl == null) 
        {
            week.EventsUrl = BuildEventsUrl(week, league.LeagueType);
            return await GetUpdatedEvents(week.EventsUrl, league);
        }

        var updatedEvents = await GetUpdatedEvents(week.EventsUrl, league);
        if (updatedEvents?.EventUrls == null || !updatedEvents.EventUrls.Any())
        {
            week.EventsUrl = BuildEventsUrlFromLeagueUrl(league);
            return await GetUpdatedEvents(week.EventsUrl, league);
        }

        return updatedEvents;
    }

    // Old potentially overcomplicated setup
    private async Task<ApiWeekEvent> GetUpdatedEvents(
        ApiUrl weekEventUrl,
        ApiLeague leagueUrl)
    {
        var cacheKey = GetCacheKey(weekEventUrl);
        var weekEventsFromCache = await GetFromCache(cacheKey);
        if (weekEventsFromCache != null)
        {
            var fromApi = await GetFromApi(weekEventUrl);
            if (fromApi != null && fromApi.EventCount != weekEventsFromCache.EventCount)
            {
                _cacheService.Remove<ApiWeekEvent>(cacheKey, CacheCategory.Json);
                return await GetUpdatedEvents(weekEventUrl, leagueUrl);
            }

            return weekEventsFromCache;
        }

        var weekEventsFromApi = await GetFromApi(weekEventUrl);
        if (weekEventsFromApi != null)
        {
            if (weekEventsFromApi.PageCount > 1)
            {
                await SetWeekEventUrlsFromApi(weekEventUrl, weekEventsFromApi);
            }

            return await SetCache(weekEventsFromApi, cacheKey);
        }

        return null;
    }

    private ApiUrl BuildEventsUrl(ApiWeek week, LeagueType leagueType)
    {
        var weekUri = new Uri(week.Url);
        var queryAddition = leagueType == LeagueType.CollegeFootball ? "&groups=80" : string.Empty;

        return BuildEventsUrl(weekUri, queryAddition);
    }

    private ApiUrl BuildEventsUrl(Uri uri, string queryAddition = "")
    {
        return new ApiUrl
        {
            Url = $"https://{uri.Host}{uri.AbsolutePath}/{ApiService<ApiWeekEvent>.EventsPath}{uri.Query}{queryAddition}"
        };
    }

    public ApiUrl BuildEventsUrlFromLeagueUrl(ApiLeague leagueUrl)
    {
        var weekUri = new Uri(leagueUrl.Url);
        return BuildEventsUrl(weekUri);
    }

    private async Task SetWeekEventUrlsFromApi(
        ApiUrl weekEventsUrl,
        ApiWeekEvent apiWeekEvent)
    {
        var startingPage = apiWeekEvent.PageIndex + 1;
        for (var currentPage = startingPage; currentPage <= apiWeekEvent.PageCount; currentPage++)
        {
            try
            {
                var nextUrl = new ApiUrl
                {
                    Url = $"{weekEventsUrl.Url}&page={currentPage}"
                };
                var nextEvents = await GetFromApi(nextUrl);
                Parallel.ForEach(nextEvents.EventUrls, apiWeekEvent.EventUrls.Enqueue);
            }
            catch 
            {
            }
        }
    }
}
