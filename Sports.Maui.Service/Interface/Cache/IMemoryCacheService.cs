namespace Sports.Maui.Service.Interface.Cache;

using Sports.Maui.Model.Interface;

public interface IMemoryCacheService
{
    T? Get<T>(string key)
        where T : class, ICacheItem;
    T Save<T>(string key, T item)
        where T : class, ICacheItem;

    void Remove(string key);
}
