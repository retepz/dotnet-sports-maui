namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;

public interface IApiCacheItemService<TCacheItem>
    where TCacheItem : class, IApiCacheItem
{
    Task<TCacheItem?> Get(ApiUrl leagueApiUrl);
    Task<(bool FromCache, TCacheItem? Item)> GetCacheOrApiResult(ApiUrl leagueApiUrl);
}
