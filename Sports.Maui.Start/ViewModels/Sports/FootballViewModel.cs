namespace Sports.Maui.Start.ViewModels.Sports;

using global::Sports.Maui.Model;
using global::Sports.Maui.Service.Interface.Api;
using global::Sports.Maui.Service.Interface.Cache;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using global::Sports.Maui.Start.Pages.SelectedLeague;

public class FootballViewModel(
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
    public override SportType SportType => SportType.Football;

    public override SelectedLeaguePage GetLeaguePage(UISportLeague uISportLeague)
    {
        return uISportLeague.LeagueType switch
        {
            LeagueType.NFL => _serviceProvider.GetService<NflSelectedLeaguePage>(),
            _ => base.GetLeaguePage(uISportLeague),
        };
    }
}
