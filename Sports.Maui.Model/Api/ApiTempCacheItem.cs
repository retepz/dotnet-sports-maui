namespace Sports.Maui.Model.Api;

public class ApiTempCacheItem(string url) : ApiCacheItem(url)
{
    public ApiTempCacheItem(ApiUrl apiUrl)
        : this(apiUrl.Url) 
    {
    }
}
