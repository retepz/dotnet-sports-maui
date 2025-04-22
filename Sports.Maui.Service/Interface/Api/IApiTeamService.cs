namespace Sports.Maui.Service.Interface.Api;

using Sports.Maui.Model.Api;

public interface IApiTeamService : IApiCacheItemService<ApiTeam>
{
    Task<ApiTeam?> Get(
        ApiCompetition competition,
        ApiCompetitor competitor);
}
