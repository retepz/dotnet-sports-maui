namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiSport : ApiCacheItem, IApiId
{
    public string Id { get; set; }

    public string DisplayName { get; set; }

    [JsonProperty("leagues")]
    public ApiUrl GetLeaguesUrl { get; set; }

    public ApiLogo[] Logos { get; set; }

    [JsonIgnore]
    public ApiLogo? Logo => Logos?.FirstOrDefault(logo => logo.IsDark) ?? Logos?.FirstOrDefault();
}
