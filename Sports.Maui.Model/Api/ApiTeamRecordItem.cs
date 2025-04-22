namespace Sports.Maui.Model.Api;

using Sports.Maui.Model.Interface.Api;

public class ApiTeamRecordItem : ApiCacheItem, IApiId
{
    public string Id { get; set; }
    public string DisplayValue { get; set; }
}
