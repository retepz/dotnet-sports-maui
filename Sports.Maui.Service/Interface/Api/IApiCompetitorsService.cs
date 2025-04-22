namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiCompetitorsService : IApiCacheItemService<ApiCompetitor>
{
    Task<IEnumerable<ApiCompetitor>?> Get(ApiCompetition competition);
}
