namespace Sports.Maui.Service.Cache;

using Microsoft.Extensions.Caching.Memory;
using Sports.Maui.Model.Interface;
using Sports.Maui.Service.Interface.Cache;

public class MemoryCacheService(IMemoryCache memoryCache) : IMemoryCacheService
{
    public T? Get<T>(string key)
        where T : class, ICacheItem
    {
        if (memoryCache.TryGetValue<T>(key, out var result))
        {
            return result;
        }

        return null;
    }

    public T Save<T>(string key, T item)
        where T : class, ICacheItem
    {
        return memoryCache.Set(key, item);
    }

    public void Remove(string key) 
    {
        memoryCache.Remove(key);
    }
}
