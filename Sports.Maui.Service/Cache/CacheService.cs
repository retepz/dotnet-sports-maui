namespace Sports.Maui.Service.Cache;

using FastHashes;
using Sports.Maui.Model;
using Sports.Maui.Model.Interface;
using Sports.Maui.Service.Interface.Cache;
using System.Text;

public class CacheService(
    IFileCacheService fileCacheService,
    IMemoryCacheService memoryCacheService) : ICacheService
{
    public readonly int CurrentVersion = 3;

    public string GetUrlCacheKey<T>(T item)
        where T : ICacheItemUrl
    {
        var urlAsBytes = Encoding.UTF8.GetBytes(item.Url);
        var hasher = new FarmHash32();
        var urlHashBytes = hasher.ComputeHash(urlAsBytes);
        var urlHash = Convert.ToHexString(urlHashBytes);

        return urlHash;
    }

    public async Task<T?> Get<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
        var memoryCache = memoryCacheService.Get<T>(key);
        var existsInFileCache = fileCacheService.ExistsInCache<T>(key, category);

        if (memoryCache != null && IsValid(memoryCache) && existsInFileCache)
        {
            return memoryCache;
        }

        if (!existsInFileCache)
        {
            return null;
        }

        var fileCache = await fileCacheService.Get<T>(key, category);

        if (fileCache != null && IsValid(fileCache) && memoryCache == null)
        {
            return memoryCacheService.Save(key, fileCache);
        }

        return null;
    }

    public async Task<T> Save<T>(T item, string key, CacheCategory category)
        where T : class, ICacheItem
    {
        item.CurrentCacheVersion = CurrentVersion;
        item.CacheDate = DateTime.UtcNow;

        var memoryCache = memoryCacheService.Save(key, item);
        await fileCacheService.Save(memoryCache, key, category);

        return memoryCache;
    }

    public void Remove<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
        memoryCacheService.Remove(key);
        fileCacheService.Remove<T>(key, category);
    }

    private bool IsValid(ICacheItem cacheItem)
    {
        return CacheVersionIsValid(cacheItem) && CacheDateIsValid(cacheItem);
    }

    private bool CacheDateIsValid(ICacheItem cacheItem)
    {
        if (cacheItem.CacheNeverExpires)
        {
            return true;
        }

        var currentDate = DateTime.UtcNow;
        var datesMatch = cacheItem.CacheDate.HasValue
            && currentDate.Year == cacheItem.CacheDate.Value.Year
                && currentDate.Month == cacheItem.CacheDate.Value.Month
                && currentDate.Day == cacheItem.CacheDate.Value.Day;

        return datesMatch;
    }

    private bool CacheVersionIsValid(ICacheItem cacheItem)
    {
        return !cacheItem.CurrentCacheVersion.HasValue || cacheItem.CurrentCacheVersion.Value == CurrentVersion;
    }
}
