namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiCompetitionService : IApiCacheItemService<ApiCompetition>
{
    Task<IEnumerable<ApiCompetition>> Get(ApiEvent apiEvent);
}
