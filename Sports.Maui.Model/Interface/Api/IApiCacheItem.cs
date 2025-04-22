namespace Sports.Maui.Model.Interface.Api;
public interface IApiCacheItem : IApiUrl, ICacheItemUrl, IApiItem
{
    bool IgnoreCache { get; }
}
