namespace Sports.Maui.Start.Views;

using Sports.Maui.Start.Model.UI;

public partial class MatchTemplateView : Grid
{
    public static readonly BindableProperty MatchProperty =
        BindableProperty.Create(nameof(Match), typeof(UIMatch), typeof(MatchTemplateView));

    public MatchTemplateView()
    {
        InitializeComponent();
    }

    public UIMatch Match
    {
        get => (UIMatch)GetValue(MatchProperty);
        set => SetValue(MatchProperty, value);
    }
}