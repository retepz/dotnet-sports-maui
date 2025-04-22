namespace Sports.Maui.Start.ViewModels.Sports;

using global::Sports.Maui.Model;
using global::Sports.Maui.Service.Interface.Api;
using global::Sports.Maui.Service.Interface.Cache;
using global::Sports.Maui.Start.Interface.Service;

public class HockeyViewModel(
    IApiSportService apiSportService,
    IServiceProvider serviceProvider,
    IPagePreferenceService pagePreferenceService,
    IFileCacheService fileCacheService)
    : SportViewModel(
        apiSportService,
        serviceProvider,
        pagePreferenceService,
        fileCacheService)
{
    public override SportType SportType => SportType.Hockey;
}
