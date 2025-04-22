namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;

public interface IApiSportService
{
    Task<ApiSport?> Get(SportType sportType);
    Task<IList<ApiLeague>?> GetLeagues(ApiSport apiSport);
}
