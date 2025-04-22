namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiTeam : ApiCacheItem, IApiId
{
    public string Id { get; set; }
    public string Name { get; set; }

    [JsonProperty("displayName")]
    public string FullName { get; set; }
    public string ShortDisplayName { get; set; }
    public string Location { get; set; }
    public string Color { get; set; }
    public string AlternateColor { get; set; }
    public ApiLogo[] Logos { get; set; }

    [JsonProperty("record")]
    public ApiUrl RecordUrl { get; set; }

    public ApiTeamRecord CurrentRecord { get; set; }

    public override bool CacheNeverExpires => true;

    [JsonIgnore]
    public ApiLogo? DefaultLogo
    {
        get
        {
            if (Logos == null || Logos.Length == 0)
            {
                return null;
            }

            return Logos.FirstOrDefault(l => l.IsDark) ?? Logos[0];
        }
    }

    [JsonIgnore]
    public ApiLogo? ScoreboardLogo 
    { 
        get 
        { 
            if(Logos == null || Logos.Length == 0)
            {
                return null;
            }

            return Logos.FirstOrDefault(l => l.IsDark && l.IsScoreboard);
        }
    }
}
