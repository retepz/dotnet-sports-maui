namespace Sports.Maui.Model.Api;

public class ApiBroadcastMedia : ApiCacheItem
{
    public string ShortName { get; set; }

    public override bool CacheNeverExpires => true;
}
