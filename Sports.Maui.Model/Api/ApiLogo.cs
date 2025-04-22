namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;

public class ApiLogo
{
    [JsonProperty("href")]
    public string Url { get; set; }

    [JsonProperty("rel")]
    public string[] MetaData { get; set; }

    [JsonIgnore]
    public bool IsDark => MetaData.Any(md => md.Contains("dark", StringComparison.OrdinalIgnoreCase));

    [JsonIgnore]
    public bool IsScoreboard => MetaData.Any(md => md.Contains("scoreboard", StringComparison.OrdinalIgnoreCase));
}
