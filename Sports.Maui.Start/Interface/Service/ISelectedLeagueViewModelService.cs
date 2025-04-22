namespace Sports.Maui.Start.Interface.Service;

public interface ISelectedLeagueViewModelService
{
    ILeaguePreferenceService LeaguePreferenceService { get; }
    IUILeagueService UILeagueService { get; }
    IServiceProvider ServiceProvider { get; }
    IPagePreferenceService PagePreferenceService { get; }
}
