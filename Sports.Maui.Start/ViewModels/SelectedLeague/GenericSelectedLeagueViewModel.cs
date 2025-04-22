namespace Sports.Maui.Start.ViewModels.SelectedLeague;

using global::Sports.Maui.Model;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;

public class GenericSelectedLeagueViewModel(
    ISelectedLeagueViewModelService selectedLeagueViewModelService) 
        : SelectedLeagueViewModel(
            selectedLeagueViewModelService)
{
    public override LeagueType LeagueType => ApiLeague?.LeagueType ?? LeagueType.None;

    protected override bool MatchesCustomTeams(UIMatch match)
    {
        throw new NotImplementedException();
    }

    protected override Task OnCustomTeamsSelected(bool selected)
    {
        throw new NotImplementedException();
    }
}