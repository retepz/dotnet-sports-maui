namespace Sports.Maui.Model.Api;

using System.Text.Json.Serialization;

public class ApiTeamRecord : ApiCacheItem
{
    public ApiTeamRecordItem[] Items { get; set; }

    [JsonIgnore]
    public string DisplayValue => Items != null && Items.Any() ? Items[0].DisplayValue : string.Empty;
}
