namespace Sports.Maui.Start.ViewModels.SelectedLeague;

using global::Sports.Maui.Model;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;

public class NflSelectedLeagueViewModel(
    ISelectedLeagueViewModelService selectedLeagueViewModelService) : SelectedLeagueViewModel(selectedLeagueViewModelService)
{
    public override LeagueType LeagueType => LeagueType.NFL;

    protected override bool MatchesCustomTeams(UIMatch match)
    {
        return false;
    }

    protected override async Task OnCustomTeamsSelected(bool selected)
    {
        await Task.CompletedTask;
    }
}
