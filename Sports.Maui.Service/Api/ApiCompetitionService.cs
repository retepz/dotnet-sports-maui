namespace Sports.Maui.Service.Api;

using Sports.Maui.Model.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;
using System.Collections.Concurrent;

public class ApiCompetitionService(
    IApiCacheItemService<ApiBroadcast> broadcastService,
    IApiCacheItemService<ApiStatus> statusService,
    IApiCompetitorsService competitorsService,
    IApiCacheItemService<ApiSituation> situationCacheService,
    IApiService<ApiSituation> situationApiService,
    ICacheService cacheService,
    IApiService<ApiCompetition> apiService) 
    : ApiCacheItemService<ApiCompetition>(cacheService, apiService), IApiCompetitionService
{
    public async Task<IEnumerable<ApiCompetition>> Get(ApiEvent apiEvent)
    {
        var results = new ConcurrentBag<ApiCompetition>();
        await Parallel.ForEachAsync(apiEvent.CompetitionUrls, async (compUrl, _cancelToken) =>
        {
            var competition = await Get(compUrl);
            if (competition == null)
            {
                return;
            }

            await SetCompetitionValues(competition);

            if (!IsValid(competition))
            {
                return;
            }

            results.Add(competition);
        });

        return results;
    }
    private async Task SetCompetitionValues(ApiCompetition competition)
    {
        // Check status first because multiple things depend on it for cache purposes.
        competition.CurrentStatus = await statusService.Get(competition.StatusUrl);
        competition.CurrentBroadcast = await broadcastService.Get(competition.BroadcastUrl);
        competition.CurrentCompetitors = await competitorsService.Get(competition);
        competition.CurrentSituation = await GetSituation(competition);
    }

    private async Task<ApiSituation?> GetSituation(ApiCompetition competition) 
    {
        if (competition.SituationUrl == null)
        {
            return null;
        }

        if (competition.CurrentStatus.IsInProgress)
        {
            return await situationApiService.Get(competition.SituationUrl);
        }

        return await situationCacheService.Get(competition.SituationUrl);
    }

    private bool IsValid(ApiCompetition apiCompetition)
    {
        return ApiValidator.IdIsValid(apiCompetition)
            && apiCompetition.CurrentCompetitors != null && apiCompetition.CurrentCompetitors.Any();
    }
}
