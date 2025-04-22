namespace Sports.Maui.Start.Views;

public partial class MatchTeamBorderView : Border
{
    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(MatchTeamBorderView));

    public MatchTeamBorderView()
	{
		InitializeComponent();
	}

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
}