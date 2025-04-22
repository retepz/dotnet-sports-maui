namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;

public interface IApiEventService : IApiCacheItemService<ApiEvent>
{
    Task<IEnumerable<ApiEvent>> Get(
             LeagueType leagueType,
             IApiEventCollection eventCollection);
}
