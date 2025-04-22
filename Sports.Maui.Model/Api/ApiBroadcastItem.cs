namespace Sports.Maui.Model.Api;
public class ApiBroadcastItem
{
    public string Station { get; set; }
    public string Slug { get; set; }
    public ApiBroadcastMarket Market { get; set; }
    public ApiBroadcastMedia Media { get; set; }
}
