namespace Sports.Maui.Service.Cache;

using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sports.Maui.Model;
using Sports.Maui.Model.Interface;
using Sports.Maui.Service.Config;
using Sports.Maui.Service.Interface;
using Sports.Maui.Service.Interface.Cache;

public class FileCacheService(
    IOptions<FileSystemConfig> fileSystemConfigProvider,
    IFileService fileService) : IFileCacheService
{
    private readonly FileSystemConfig _fileSystemConfig = fileSystemConfigProvider.Value;
    private readonly Dictionary<CacheCategory, bool> _infoFileCreatedMap = [];

    public async Task<T?> Get<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
        if (!ExistsInCache<T>(key, category))
        {
            return null;
        }

        await CreateInfoFileIfNeeded(category);

        var path = GetCachePath<T>(key, category);
        if (!File.Exists(path)) 
        {
            return null;
        }

        var serialized = await fileService.Decompress(path);

        var result = JsonConvert.DeserializeObject<T>(serialized, new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore
        });

        return result;
    }

    public async Task Save<T>(T item, string key, CacheCategory category)
        where T : class, ICacheItem
    {
        var serialized = JsonConvert.SerializeObject(item);

        await CreateInfoFileIfNeeded(category);
        var path = GetCachePath<T>(key, category);
        if (!File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.DirectoryName!);
        }

        await fileService.Compress(path, serialized);
    }

    public bool ExistsInCache<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
        var path = GetCachePath<T>(key, category);
        return File.Exists(path);
    }

    public void Remove<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
        var path = GetCachePath<T>(key, category);
        File.Delete(path);
    }

    public async Task TryCleanupCache()
    {
        if (!Directory.Exists(BaseCacheDirectory))
        {
            DeleteCoreCacheDirectoryContents();
            return;
        }

        foreach (var enumValue in Enum.GetValues<CacheCategory>()) 
        {
            await TryClearCache(enumValue);
        }
    }

    public async Task CreateInfoFileIfNeeded(CacheCategory category)
    {
        if(_infoFileCreatedMap.TryGetValue(category, out var alreadyCreated) && alreadyCreated)
        {
            return;
        }

        var cacheInfoFile = await GetInfoFile(category);
        if(cacheInfoFile != null)
        {
            _infoFileCreatedMap.TryAdd(category, true);
            return;
        }

        await CreateInfoFile(category);
        _infoFileCreatedMap.TryAdd(category, true);
    }

    private async Task TryClearCache(CacheCategory category) 
    {
        try
        {
            await ClearCache(category);
        }
        catch
        {
        }
    }

    private async Task ClearCache(CacheCategory category)
    {
        var cacheFileInfo = await GetInfoFile(category);
        if (cacheFileInfo != null && IsValid(cacheFileInfo))
        {
            return;
        }

        if (cacheFileInfo == null)
        {
            return;
        }

        DeleteCacheDirectory(category);
    }

    private async Task CreateInfoFile(CacheCategory category) 
    {
        var infoFilePath = GetInfoFilePath(category);
        var fileInfo = new FileInfo(infoFilePath);
        if (!Directory.Exists(fileInfo.DirectoryName))
        {
            Directory.CreateDirectory(fileInfo.DirectoryName!);
        }

        var info = new CacheFileInfo();
        var serialized = JsonConvert.SerializeObject(info);
        await fileService.Compress(infoFilePath, serialized);
    }

    private void DeleteCacheDirectory(CacheCategory category)
    {
        var cachePath = GetBasePath(category);
        if (!Directory.Exists(cachePath))
        {
            return;
        }

        Directory.Delete(cachePath, true);
    }

    private async Task<CacheFileInfo?> GetInfoFile(CacheCategory category)
    {
        var infoFilePath = GetInfoFilePath(category);
        if (!File.Exists(infoFilePath))
        {
            return null;
        }

        var decompressed = await fileService.Decompress(infoFilePath);

        return JsonConvert.DeserializeObject<CacheFileInfo>(decompressed)!;
    }

    private bool IsValid(CacheFileInfo info)
    {
        var createdPlusPadding = info.Created.AddDays(2);
        return DateTime.UtcNow < createdPlusPadding;
    }

    private string GetInfoFilePath(CacheCategory category) 
    {
        var path = GetBasePath(category);
        return Path.Combine(path, "info.json.gz");
    }

    private string GetFileName(string key, CacheCategory category, bool isZipped = true) 
    {
        var categoryAsString = category.ToString().ToLower();
        var zippedExtension = isZipped ? ".gz" : string.Empty;

        return $"{key}.{categoryAsString}{zippedExtension}";
    }

    private void DeleteCoreCacheDirectoryContents()
    {
        if (!Directory.Exists(_fileSystemConfig.CacheDirectory))
        {
            return;
        }

        fileService.DeleteDirectoryContents(_fileSystemConfig.CacheDirectory);
    }

    private string BaseCacheDirectory => Path.Combine(_fileSystemConfig.CacheDirectory, "sports.maui");
    private string GetBasePath(CacheCategory category)
    {
        var categoryDirectory = category.ToString().ToLower();
        return Path.Combine(BaseCacheDirectory, categoryDirectory);
    }

    private string GetBasePath<T>(CacheCategory category)
        where T : class, ICacheItem
    {
        var cacheType = typeof(T).Name.ToLower();
        return Path.Combine(GetBasePath(category), cacheType);
    }

    private string GetCachePath<T>(string key, CacheCategory category)
        where T : class, ICacheItem
    {
    
        var basePath = GetBasePath<T>(category);

        return Path.Combine(basePath, GetFileName(key, category));
    }
}
