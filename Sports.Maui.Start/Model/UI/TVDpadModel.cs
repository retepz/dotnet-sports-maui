namespace Sports.Maui.Start.Model.UI;

public abstract class TVDpadModel : DpadModel
{
    private string _name;
    private string? _logo;
    private bool? _isEnabled;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NotifyPropertyChanged(nameof(Name));
        }
    }

    public string? Logo
    {
        get => _logo;
        set
        {
            _logo = value;
            NotifyPropertyChanged(nameof(Logo));
        }
    }

    public bool? IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
            NotifyPropertyChanged(nameof(IsEnabled));
        }
    }
}
