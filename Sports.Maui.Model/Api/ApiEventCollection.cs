namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;
using System.Collections.Concurrent;

public abstract class ApiEventCollection : ApiCacheItem, IApiEventCollection
{
    [JsonProperty("items")]
    public ConcurrentQueue<ApiUrl> EventUrls { get; set; }

    [JsonProperty("count")]
    public int EventCount { get; set; }
    public int PageCount { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}
