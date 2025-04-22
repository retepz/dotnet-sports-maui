namespace Sports.Maui.Start.Service;

using Sports.Maui.Start.Interface.Service;

public class SelectedLeagueViewModelService(ILeaguePreferenceService leaguePreferenceService,
        IUILeagueService uiLeagueService,
        IServiceProvider serviceProvider,
        IPagePreferenceService pagePreferenceService) : ISelectedLeagueViewModelService
{
    public ILeaguePreferenceService LeaguePreferenceService => leaguePreferenceService;
    public IUILeagueService UILeagueService => uiLeagueService;
    public IServiceProvider ServiceProvider => serviceProvider;
    public IPagePreferenceService PagePreferenceService => pagePreferenceService;
}
