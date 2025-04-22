namespace Sports.Maui.Start.Model.UI;

using System.Collections.ObjectModel;

public class UIMatchGroup : ObservableCollection<UIMatch>
{
    public string Name { get; }

    public UIMatchGroup(string name)
    {
        Name = name;
    }

    public IList<UIMatch> GetItems()
    {
        return Items;
    }
}
