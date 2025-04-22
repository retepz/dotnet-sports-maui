namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;

public interface IApiService<T>
    where T : class, IApiUrl
{
    Task<T?> Get(ApiUrl apiUrl);
}
