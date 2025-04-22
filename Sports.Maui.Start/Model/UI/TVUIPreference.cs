namespace Sports.Maui.Start.Model.UI;

using Sports.Maui.Model;

public class TVUIPreference(UserPreferenceKey userPreferenceKey) : TVDpadModel
{
    public UserPreferenceKey UserPreferenceKey { get; } = userPreferenceKey;
}
