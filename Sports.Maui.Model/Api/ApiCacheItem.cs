namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public abstract class ApiCacheItem : ApiUrl, IApiCacheItem
{
    public ApiCacheItem() { }
    public ApiCacheItem(string url)
    {
        Url = url;
    }

    public int? CurrentCacheVersion { get; set; }
    public DateTime? CacheDate { get; set; }

    [JsonIgnore]
    public virtual bool CacheNeverExpires { get; }

    [JsonIgnore]
    public virtual bool IgnoreCache => false;
}
