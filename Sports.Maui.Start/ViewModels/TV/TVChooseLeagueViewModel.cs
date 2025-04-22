namespace Sports.Maui.Start.ViewModels.TV;

using global::Sports.Maui.Model;
using global::Sports.Maui.Model.Api;
using global::Sports.Maui.Service.Interface.Api;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using global::Sports.Maui.Start.Pages.SelectedLeague;
using System.Collections.ObjectModel;

public class TVChooseLeagueViewModel(
    IServiceProvider serviceProvider,
    IPagePreferenceService pagePreferenceService,
    IApiSportService apiSportService) : TVBasePageViewModel<UISportLeague>
{
    private UISport _sport;
    private ApiSport _apiSport;

    protected override IList<UISportLeague> Collection => AvailableLeagues;

    public ObservableCollection<UISportLeague> AvailableLeagues { get; } = [];

    public override async Task OnInitialPageLoad()
    {
        var lastVisitedLeagueId = pagePreferenceService.GetLastVisitedLeagueId(_apiSport);
        UISportLeague? lastVisitedLeague = null;

        var apiLeagues = await apiSportService.GetLeagues(_apiSport);
        if(apiLeagues == null)
        {
            await OnPageAppearing();
            return;
        }

        foreach (var apiLeague in apiLeagues)
        {
            var sportLeague = new UISportLeague(apiLeague);
            AvailableLeagues.Add(sportLeague);

            if(string.IsNullOrEmpty(lastVisitedLeagueId) || apiLeague.Id != lastVisitedLeagueId) 
            {
                continue;
            }

            lastVisitedLeague = sportLeague;
        }

        if(lastVisitedLeague != null)
        {
            await OnSelected(lastVisitedLeague);
            return;
        }

        await OnPageAppearing();
    }

    public void SetApiSport(ApiSport apiSport)
    {
        _apiSport = apiSport;
        Sport = new(apiSport);
    }

    public UISport Sport
    {
        get => _sport;
        set
        {
            _sport = value;
            NotifyPropertyChanged(nameof(Sport));
        }
    }

    protected override async Task OnAppearing()
    {
        pagePreferenceService.DeleteLastVisitedLeagueId(_apiSport);
        await Task.CompletedTask;
    }

    protected override async Task OnSelected(UISportLeague? league)
    {
        await GoToLeaguePage(league);
    }

    protected async Task GoToLeaguePage(UISportLeague? league)
    {
        pagePreferenceService.SetLastVisitedLeagueId(_apiSport, league!.ApiLeague);

        SelectedLeaguePage selectedPage = league.LeagueType switch
        {
            LeagueType.NFL => serviceProvider.GetRequiredService<NflSelectedLeaguePage>(),
            _ => serviceProvider.GetRequiredService<GenericSelectedLeaguePage>()
        };

        selectedPage.SetApiLeague(league.ApiLeague);

        await NavigateTo(selectedPage);
    }
}