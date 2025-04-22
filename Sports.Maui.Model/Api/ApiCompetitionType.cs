namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiCompetitionType : ApiCacheItem, IApiId
{
    public string Id { get; set; }

    [JsonProperty("text")]
    public string DisplayName { get; set; }

    [JsonProperty("abbreviation")]
    public string ShortName { get; set; }

    [JsonProperty("type")]
    public string Name { get; set; }

    public override bool CacheNeverExpires => true;

    [JsonIgnore]
    public ApiCompetitionTypeName CurrentName
    {
        get
        {
            if (Enum.TryParse<ApiCompetitionTypeName>(Name, out var typeName))
            {
                return typeName;
            }

            return ApiCompetitionTypeName.none;
        }
    }
}
