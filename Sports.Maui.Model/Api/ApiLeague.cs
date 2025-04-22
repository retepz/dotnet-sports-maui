namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiLeague : ApiCacheItem, IApiId
{
    public string? Id { get; set; }

    public LeagueType LeagueType { get; set; }

    public string? ShortName { get; set; }

    public string? DisplayName { get; set; }

    public string? Name { get; set; }

    public string? Abbreviation { get; set; }

    public ApiLogo[] Logos { get; set; }

    [JsonProperty("season")]
    public ApiLeagueSeason? CurrentSeason { get; set; }

    [JsonIgnore]
    public ApiLogo? Logo => Logos?.FirstOrDefault(logo => logo.IsDark) ?? Logos?.FirstOrDefault();
}
