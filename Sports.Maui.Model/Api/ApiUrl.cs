namespace Sports.Maui.Model.Api;

using Newtonsoft.Json;
using Sports.Maui.Model.Interface.Api;

public class ApiUrl : IApiUrl
{
    private string? _url;

    [JsonProperty("$ref")]
    public string Url 
    {
        get => _url!;
        set
        {
            if (value == null) 
            {
                return;
            }

            if (value!.StartsWith("https"))
            {
                _url = value;
                return;
            }

            _url = value.Replace("http", "https");
        }
    }
}
