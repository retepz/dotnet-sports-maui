namespace Sports.Maui.Start.Interface.Service;

using Sports.Maui.Model.Api;
using Sports.Maui.Start.Model.UI;

public interface IUILeagueService
{
    Task<UILeague?> GetCurrentSeason(ApiLeague league);
    Task SetMatches(UILeague uiLeague);
}
