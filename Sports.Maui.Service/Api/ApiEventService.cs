namespace Sports.Maui.Service.Api;

using Sports.Maui.Model;
using Sports.Maui.Model.Api;
using Sports.Maui.Model.Interface.Api;
using Sports.Maui.Service.Interface.Api;
using Sports.Maui.Service.Interface.Cache;
using System.Collections.Concurrent;

public class ApiEventService(
    IApiCompetitionService apiCompetitionService,
    ICacheService cacheService,
    IApiService<ApiEvent> apiService) 
    : ApiCacheItemService<ApiEvent>(cacheService, apiService), IApiEventService
{
    public async Task<IEnumerable<ApiEvent>> Get(
     LeagueType leagueType,
     IApiEventCollection eventCollection)
    {
        var results = new ConcurrentBag<ApiEvent>();

        await Parallel.ForEachAsync(eventCollection.EventUrls, async (item, _cancelToken) =>
        {
            var apiEvent = await Get(item);
            if (apiEvent == null)
            {
                return;
            }

            apiEvent.LeagueType = leagueType;
            apiEvent.CurrentCompetitions = await apiCompetitionService.Get(apiEvent);

            if (!IsValid(apiEvent))
            {
                return;
            }

            results.Add(apiEvent);
        });

        return GetOrderedEvents(results);
    }

    private bool IsValid(ApiEvent apiEvent)
    {
        return ApiValidator.IdIsValid(apiEvent)
            && apiEvent.CurrentCompetitions != null && apiEvent.CurrentCompetitions.Any();
    }

    private IEnumerable<ApiEvent> GetOrderedEvents(IEnumerable<ApiEvent> events)
    {
        return events
            .OrderBy(nc => nc.Competition.Date)
            .ThenBy(nc => nc.Competition.FirstCompetitor.CurrentTeam.ShortDisplayName);
    }
}
