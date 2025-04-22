namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model.Api;

public class UISport : TVDpadModel
{
    public UISport(ApiSport apiSport)
    {
        ApiSport = apiSport;
        Name = apiSport.DisplayName;
        Logo = apiSport.Logo?.Url;
    }

    public UISport(string name)
    {
        Name = name;
        Logo = "magnifying_glass.png";
    }

    public ApiSport? ApiSport { get; }
}
