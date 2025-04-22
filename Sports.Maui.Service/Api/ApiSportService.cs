namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiSportService(
    ICacheService cacheService,
    IApiService<ApiSport> apiService,
    IApiLeagueService apiLeagueService,
    IApiCacheItemService<ApiSportLeagues> apiSportLeaguesService) 
    : ApiCacheItemService<ApiSport>(cacheService, apiService), IApiSportService
{
    private const string _baseUrl = "https://sports.core.api.espn.com/v2/sports";

    public async Task<ApiSport?> Get(SportType sportType)
    {
        var apiUrl = new ApiUrl
        {
            Url = $"{_baseUrl}/{sportType.ToString().ToLower()}"
        };

        return await Get(apiUrl);
    }

    public async Task<IList<ApiLeague>?> GetLeagues(ApiSport apiSport)
    {
        if(apiSport.GetLeaguesUrl == null)
        {
            return null;
        }

        ApiSportLeagues leagueUrls = null;
        try
        {
            leagueUrls = await apiSportLeaguesService.Get(apiSport.GetLeaguesUrl);
        }
        catch (Exception ex) 
        {
        }

        if(leagueUrls == null)
        {
            return null;
        }

        var result = new List<ApiLeague>();
        foreach (var apiLeagueUrl in leagueUrls.Items)
        {
            var apiLeague = await apiLeagueService.Get(apiLeagueUrl);
            if (apiLeague == null)
            {
                continue;
            }

            result.Add(apiLeague);
        }

        return result;
    }
}
