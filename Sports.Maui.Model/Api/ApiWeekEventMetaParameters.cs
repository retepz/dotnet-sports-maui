namespace Sports.Maui.Model.Api;

using System.Text.Json.Serialization;

public class ApiWeekEventMetaParameters
{
    public string[] Week { get; set; }
    public string[] Season { get; set; }
    public string[] SeasonTypes { get; set; }
}
