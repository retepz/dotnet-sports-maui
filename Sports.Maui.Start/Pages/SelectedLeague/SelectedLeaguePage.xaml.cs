namespace Sports.Maui.Start.Pages.SelectedLeague;

using global::Sports.Maui.Model;
using global::Sports.Maui.Model.Api;
using global::Sports.Maui.Start.ViewModels.SelectedLeague;

[XamlCompilation(XamlCompilationOptions.Compile)]
public abstract partial class SelectedLeaguePage : BasePage
{
    public SelectedLeaguePage(
        SelectedLeagueViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetCollectionView(MatchesCollectionView);
        SetViewModel(viewModel);
    }

    protected LeagueType LeagueType => GetViewModel<SelectedLeagueViewModel>().LeagueType;

    public void SetApiLeague(ApiLeague league)
    {
        GetViewModel<SelectedLeagueViewModel>().SetApiLeague(league);
    }
}