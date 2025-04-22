namespace Sports.Maui.Start.ViewModels.TV;

using global::Sports.Maui.Model;
using global::Sports.Maui.Service.Interface.Api;
using global::Sports.Maui.Service.Interface.Cache;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using global::Sports.Maui.Start.Pages.TV;
using System.Collections.ObjectModel;

public class TVChooseSportViewModel(
    IApiSportService apiSportService,
    IServiceProvider serviceProvider,
    IPagePreferenceService pagePreferenceService,
    IFileCacheService fileCacheService) : TVBasePageViewModel<UISport>
{
    protected override IList<UISport> Collection => AvailableSports;

    public ObservableCollection<UISport> AvailableSports { get; set; } = [];

    public override async Task OnInitialPageLoad()
    {
        await Task.Run(fileCacheService.TryCleanupCache);

        var lastVisitedSportId = pagePreferenceService.GetLastVisitedSportId();
        UISport lastVisitedSport = null;

        AvailableSports.Add(new UISport("Preferences"));

        foreach (var sportType in Enum.GetValues<SportType>())
        {
            if (sportType == SportType.None)
            {
                continue;
            }

            var sport = await apiSportService.Get(sportType);
            var uiSport = new UISport(sport);

            AvailableSports.Add(uiSport);

            if (string.IsNullOrEmpty(lastVisitedSportId) || lastVisitedSportId != sport.Id)
            {
                continue;
            }

            lastVisitedSport = uiSport;
        }

        if(lastVisitedSport != null)
        {
            await OnSelected(lastVisitedSport);
            return;
        }

        await OnPageAppearing();
    }

    protected override async Task OnAppearing()
    {
        pagePreferenceService.DeleteLastVisitedSportId();

        await Task.CompletedTask;
    }

    protected override async Task OnSelected(UISport? sport)
    {
        await GoToChooseLeaguePage(sport);
    }

    protected async Task GoToChooseLeaguePage(UISport? sport)
    {
        if(sport?.ApiSport == null)
        {
            var preferencesPage = serviceProvider.GetRequiredService<TVPreferencesPage>();
            await DisplayAsModal(preferencesPage);
            return;
        }

        pagePreferenceService.SetLastVisitedSportId(sport.ApiSport);

        var chooseLeaguePage = serviceProvider.GetRequiredService<TVChooseLeaguePage>();
        chooseLeaguePage.SetApiSport(sport.ApiSport);

        await NavigateTo(chooseLeaguePage);
    }
}