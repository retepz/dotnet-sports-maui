namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiLeagueService(
    IApiService<ApiLeague> apiService,
    IApiCacheItemService<ApiSeason> apiSeasonService,
    ICacheService cacheService,
    IApiWeekService apiWeekService,
    IApiWeekEventService weekEventService,
    IApiEventService apiEventService) 
    : ApiCacheItemService<ApiLeague>(cacheService, apiService), IApiLeagueService
{
    private readonly Dictionary<string, LeagueType> _leagueAbbreviationNameMap = new()
    {
        { "ncaam", LeagueType.CollegeBasketball },
        { "ncaaf", LeagueType.CollegeFootball }
    };

    protected override async Task<ApiLeague?> GetFromApi(ApiUrl leagueApiUrl)
    {
        var league = await base.GetFromApi(leagueApiUrl);

        if (league == null)
        {
            return null;
        }

        league.LeagueType = TryGetLeagueType(league);

        return league;
    }

    public async Task<ApiSeason?> GetCurrentSeason(ApiLeague league)
    {
        if(league.CurrentSeason == null)
        {
            return null;
        }

        return await apiSeasonService.Get(league.CurrentSeason);
    }

    public async Task<ApiWeek?> GetCurrentWeek(ApiLeague league, ApiSeason season)
    {
        return await apiWeekService.GetCurrentWeek(league, season);
    }

    public async Task<(ApiWeek?, IEnumerable<ApiEvent>?)> GetWeekEvents(ApiLeague league, ApiSeason season, ApiWeek? week)
    {
        if(week == null)
        {
            var (newWeek, newWeekWeekEvent) = await weekEventService.Get(league, season);
            var newWeekApiEvents = await apiEventService.Get(league.LeagueType, newWeekWeekEvent);
            return (newWeek, newWeekApiEvents);
        }

        var weekEvent = await weekEventService.Get(league, week);
        var apiEvents = await apiEventService.Get(league.LeagueType, weekEvent);

        return (null, apiEvents);
    }

    private LeagueType TryGetLeagueType(ApiLeague league)
    {
        if (TryParseFromAbbrev(league, out var abbrevLeague))
        {
            return abbrevLeague;
        }

        if (TryParseLeagueType(league.ShortName, out var shortNameLeague))
        {
            return shortNameLeague;
        }

        if (TryParseLeagueType(league.Name, out var nameLeague))
        {
            return nameLeague;
        }

        if (TryParseLeagueType(league.DisplayName, out var displayNameLeague))
        {
            return displayNameLeague;
        }

        return LeagueType.None;
    }
    private bool TryParseFromAbbrev(ApiLeague league, out LeagueType result)
    {
        var toParse = league.Abbreviation?.ToLower() ?? string.Empty;

        if (_leagueAbbreviationNameMap.TryGetValue(toParse, out result))
        {
            return true;
        }

        return TryParseLeagueType(league.Abbreviation, out result);
    }

    private bool TryParseLeagueType(string? input, out LeagueType result)
    {
        var toParse = input ?? string.Empty;

        if (Enum.TryParse(toParse, ignoreCase: true, out result))
        {
            return true;
        }

        var stringReplacedValue = toParse.Replace(" ", string.Empty);

        if (Enum.TryParse(stringReplacedValue, ignoreCase: true, out result))
        {
            return true;
        }

        result = LeagueType.None;
        return false;
    }
}
