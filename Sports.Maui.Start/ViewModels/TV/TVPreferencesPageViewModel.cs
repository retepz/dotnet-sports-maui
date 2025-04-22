namespace Sports.Maui.Start.ViewModels.TV;

using global::Sports.Maui.Model;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using System.Collections.ObjectModel;

public class TVPreferencesPageViewModel(IPagePreferenceService pagePreferenceService) : TVBasePageViewModel<TVUIPreference>
{
    protected override IList<TVUIPreference> Collection => Preferences;

    public ObservableCollection<TVUIPreference> Preferences { get; set; } = [];

    public override Task OnInitialPageLoad()
    {
        var shouldShowSearch = pagePreferenceService.GetShowSearchPref();
        Preferences.Add(new TVUIPreference(UserPreferenceKey.ShowSearch)
        {
            Name = "Show Search",
            IsEnabled = shouldShowSearch.HasValue && shouldShowSearch.Value
        });

        return base.OnInitialPageLoad();
    }

    protected override async Task OnAppearing()
    {
        await Task.CompletedTask;
    }

    protected override async Task OnSelected(TVUIPreference item)
    {
        var newValue = !(item.IsEnabled ?? false);
        item.IsEnabled = newValue;
        switch (item.UserPreferenceKey)
        {
            case UserPreferenceKey.ShowSearch:
                SetShowSearchPref(newValue, item);
                break;
            default:
                throw new NotImplementedException();
        }

        Opacity = 1;
        SelectedItem = null;

        await Task.CompletedTask;
    }

    private void SetShowSearchPref(bool newValue, TVUIPreference item)
    {
        pagePreferenceService.SetShowSearchPref(newValue);
    }
}