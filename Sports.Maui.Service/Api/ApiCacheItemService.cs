namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiCacheItemService<TCacheItem>(
    ICacheService cacheService,
    IApiService<TCacheItem> apiService) : IApiCacheItemService<TCacheItem>
    where TCacheItem : class, IApiCacheItem
{
    protected readonly ICacheService _cacheService = cacheService;

    public async virtual Task<TCacheItem?> Get(ApiUrl leagueApiUrl)
    {
        var (_, result) = await GetCacheOrApiResult(leagueApiUrl);

        return result;
    }

    public async virtual Task<(bool FromCache, TCacheItem? Item)> GetCacheOrApiResult(
        ApiUrl leagueApiUrl)
    {
        var cacheKey = GetCacheKey(leagueApiUrl);

        var fromCache = await GetFromCache(cacheKey);
        if (fromCache != null)
        {
            return (true, fromCache);
        }

        var fromApi = await TryGetFromApi(leagueApiUrl);
        if (fromApi == null)
        {
            return (false, null);
        }

        var updatedApiCache = await SetCache(fromApi, cacheKey);
        return(false, updatedApiCache);
    }

    protected async Task<TCacheItem?> GetFromCache(string cacheKey)
    {
        var fromCache = await _cacheService.Get<TCacheItem>(cacheKey, CacheCategory.Json);
        if (fromCache != null && !fromCache.IgnoreCache)
        {
            return fromCache;
        }

        return  null;
    }

    protected string GetCacheKey(ApiUrl leagueApiUrl)
    {
        var tempForCache = new ApiTempCacheItem(leagueApiUrl.Url);
        var cacheKey = _cacheService.GetUrlCacheKey(tempForCache);
        return cacheKey;
    }

    protected async Task<TCacheItem?> SetCache(TCacheItem fromApi, string cacheKey)
    {
        var updatedItem = await _cacheService.Save(fromApi, cacheKey, CacheCategory.Json);
        return updatedItem;
    }

    protected virtual async Task<TCacheItem?> GetFromApi(ApiUrl apiUrl)
    {
        return await apiService.Get(apiUrl);
    }

    protected async Task<TCacheItem?> TryGetFromApi(ApiUrl apiUrl)
    {
        try
        {
            return await GetFromApi(apiUrl);
        }
        catch(Exception e)
        {
            return null;
        }
    }
}
