namespace Sports.Maui.Service.Api;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;
using Sports.Maui.Service.Interface.Api;

public class ApiService<T> : IApiService<T>
    where T : class, IApiUrl
{
    public const string EventsPath = "events";

    public async Task<T?> Get(ApiUrl apiUrl)
    {
        using var client = new HttpClient();
        try
        {
            var result = await client.GetAsync(apiUrl.Url);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var asString = await result.Content.ReadAsStringAsync();

            DefaultContractResolver contractResolver = new()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var deserializedResult = JsonConvert.DeserializeObject<T>(asString, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore
            })!;

            if (string.IsNullOrEmpty(deserializedResult.Url))
            {
                deserializedResult.Url = apiUrl.Url;
            }

            return deserializedResult;
        }
        catch(Exception e)
        {
            return null;
        }
    }
}
