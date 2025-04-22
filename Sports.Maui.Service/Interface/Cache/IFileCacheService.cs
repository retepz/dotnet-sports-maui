namespace Sports.Maui.Service.Interface.Cache;

using Sports.Maui.Model;
using Sports.Maui.Model.Interface;

public interface IFileCacheService
{
    Task<T?> Get<T>(string key, CacheCategory category)
        where T : class, ICacheItem;
    Task Save<T>(T item, string key, CacheCategory category)
        where T : class, ICacheItem;
    void Remove<T>(string key, CacheCategory category)
        where T : class, ICacheItem;
    bool ExistsInCache<T>(string key, CacheCategory category)
        where T : class, ICacheItem;
    Task TryCleanupCache();
}
