namespace Sports.Maui.Service.Api;

using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;

public class ApiTeamService(
    ICacheService cacheService,
    IApiService<ApiTeam> apiService,
    IApiService<ApiTeamRecord> apiTeamRecordApiService,
    IApiCacheItemService<ApiTeamRecord> apiTeamRecordCacheService)
    : ApiCacheItemService<ApiTeam>(cacheService, apiService), IApiTeamService
{
    public async Task<ApiTeam?> Get(
        ApiCompetition competition,
        ApiCompetitor competitor)
    {
        var team = await Get(competitor.TeamUrl);
        if (team == null || !ApiValidator.IdIsValid(team))
        {
            return null;
        }

        team.CurrentRecord = await GetRecord(competition, team);

        return team;
    }

    public async Task<ApiTeamRecord?> GetRecord(ApiCompetition competition, ApiTeam team)
    {
        if (competition.CurrentStatus.IsInProgress)
        {
            return await apiTeamRecordApiService.Get(team.RecordUrl);
        }

        return await apiTeamRecordCacheService.Get(team.RecordUrl);
    }
}
