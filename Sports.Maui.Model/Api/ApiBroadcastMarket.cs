namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiBroadcastMarket : IApiId
{
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Name { get; set; }
}
