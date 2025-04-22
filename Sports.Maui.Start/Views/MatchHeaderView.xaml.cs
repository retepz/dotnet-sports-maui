namespace Sports.Maui.Start.Views;

public partial class MatchHeaderView : VerticalStackLayout
{
    public static readonly BindableProperty QuarterDisplayProperty =
        BindableProperty.Create(nameof(QuarterDisplay), typeof(string), typeof(MatchHeaderView));

    public static readonly BindableProperty GameTimeDisplayProperty =
        BindableProperty.Create(nameof(GameTimeDisplay), typeof(string), typeof(MatchHeaderView));

    public static readonly BindableProperty BroadcastStationsProperty =
        BindableProperty.Create(nameof(BroadcastStations), typeof(string), typeof(MatchHeaderView));

    public MatchHeaderView()
	{
		InitializeComponent();
	}

    public string QuarterDisplay
    {
        get => (string)GetValue(QuarterDisplayProperty);
        set => SetValue(QuarterDisplayProperty, value);
    }

    public string GameTimeDisplay
    {
        get => (string)GetValue(GameTimeDisplayProperty);
        set => SetValue(GameTimeDisplayProperty, value);
    }

    public string BroadcastStations
    {
        get => (string)GetValue(BroadcastStationsProperty);
        set => SetValue(BroadcastStationsProperty, value);
    }
}