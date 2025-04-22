namespace Sports.Maui.Start.Model;

public abstract class DpadModel : NotifyPropertyChangedModel
{
    private Color _hoveredColor;

    public DpadModel()
    {
        _hoveredColor = DefaultColor;
    }

    public virtual Color DefaultColor => Colors.Transparent;

    public Color HoveredColor
    {
        get => _hoveredColor;
        set
        {
            _hoveredColor = value;
            NotifyPropertyChanged(nameof(HoveredColor));
        }
    }
}
