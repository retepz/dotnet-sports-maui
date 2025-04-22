namespace Sports.Maui.Service.Api;

using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;
using System.Collections.Concurrent;

public class ApiCompetitorsService(
    IApiTeamService apiTeamService,
    IApiCacheItemService<ApiScore> apiScoreCacheService,
    IApiService<ApiScore> apiScoreApiService,
    IApiService<ApiCompetitor> apiService,
    ICacheService cacheService) : ApiCacheItemService<ApiCompetitor>(cacheService, apiService), IApiCompetitorsService
{
    public async Task<IEnumerable<ApiCompetitor>?> Get(ApiCompetition competition)
    {
        var results = new ConcurrentBag<ApiCompetitor>();

        await Parallel.ForEachAsync(competition.CompetitorUrls, async (url, _cancelToken) =>
        {
            var competitor = await Get(url);
            if (competition == null || competitor == null || !ApiValidator.IdIsValid(competitor))
            {
                return;
            }

            var currentTeam = await apiTeamService.Get(competition, competitor);
            if (currentTeam == null)
            {
                return;
            }

            competitor.CurrentTeam = currentTeam;
            competitor.CurrentScore = (await GetCurrentScore(competition, competitor))!;

            results.Add(competitor);
        });

        return !results.IsEmpty ? results : null;
    }

    private async Task<ApiScore?> GetCurrentScore(ApiCompetition competition, ApiCompetitor competitor)
    {
        if (competition.CurrentStatus.IsInProgress)
        {
            return await apiScoreApiService.Get(competitor.ScoreUrl);
        }

        return await apiScoreCacheService.Get(competitor.ScoreUrl);
    }
}
