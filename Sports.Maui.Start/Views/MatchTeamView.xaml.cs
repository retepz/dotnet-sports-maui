namespace Sports.Maui.Start.Views;
public partial class MatchTeamView : VerticalStackLayout
{
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty LogoColumnDefinitionProperty =
        BindableProperty.Create(nameof(LogoColumnDefinition), typeof(GridLength), typeof(MatchTeamView));

    public static readonly BindableProperty ScoreColumnDefinitionProperty =
    BindableProperty.Create(nameof(ScoreColumnDefinition), typeof(GridLength), typeof(MatchTeamView));

    public static readonly BindableProperty LocationProperty =
        BindableProperty.Create(nameof(Location), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty LogoProperty =
        BindableProperty.Create(nameof(Logo), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty ScoreProperty =
        BindableProperty.Create(nameof(Score), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty HomeAwayProperty =
        BindableProperty.Create(nameof(HomeAway), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty HasLogoProperty =
        BindableProperty.Create(nameof(HasLogo), typeof(bool), typeof(MatchTeamView));

    public static readonly BindableProperty HasPossessionProperty =
        BindableProperty.Create(nameof(HasPossession), typeof(bool), typeof(MatchTeamView));

    public static readonly BindableProperty RecordProperty =
        BindableProperty.Create(nameof(Record), typeof(string), typeof(MatchTeamView));

    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(MatchTeamView));

    public static readonly BindableProperty ContentHorizontalOptionsProperty =
        BindableProperty.Create(nameof(ContentHorizontalOptions), typeof(LayoutOptions), typeof(MatchTeamView));

    public static readonly BindableProperty LogoHorizontalOptionsProperty =
        BindableProperty.Create(nameof(LogoHorizontalOptions), typeof(LayoutOptions), typeof(MatchTeamView));

    public static readonly BindableProperty ContentHorizontalTextAlignmentProperty =
        BindableProperty.Create(nameof(ContentHorizontalTextAlignment), typeof(TextAlignment), typeof(MatchTeamView));

    public static readonly BindableProperty LogoColumnProperty =
        BindableProperty.Create(nameof(LogoColumn), typeof(int), typeof(MatchTeamView));

    public static readonly BindableProperty ScoreColumnProperty =
        BindableProperty.Create(nameof(ScoreColumn), typeof(int), typeof(MatchTeamView));

    public MatchTeamView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    [System.ComponentModel.TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength LogoColumnDefinition
    {
        get => (GridLength)GetValue(LogoColumnDefinitionProperty);
        set => SetValue(LogoColumnDefinitionProperty, value);
    }

    [System.ComponentModel.TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength ScoreColumnDefinition
    {
        get => (GridLength)GetValue(ScoreColumnDefinitionProperty);
        set => SetValue(ScoreColumnDefinitionProperty, value);
    }

    public string Location
    {
        get => (string)GetValue(LocationProperty);
        set => SetValue(LocationProperty, value);
    }

    public string Logo
    {
        get => (string)GetValue(LogoProperty);
        set => SetValue(LogoProperty, value);
    }

    public string Score
    {
        get => (string)GetValue(ScoreProperty);
        set => SetValue(ScoreProperty, value);
    }

    public string HomeAway
    {
        get => (string)GetValue(HomeAwayProperty);
        set => SetValue(HomeAwayProperty, value);
    }

    public bool HasLogo
    {
        get => (bool)GetValue(HasLogoProperty);
        set => SetValue(HasLogoProperty, value);
    }

    public bool HasPossession
    {
        get => (bool)GetValue(HasPossessionProperty);
        set => SetValue(HasPossessionProperty, value);
    }

    public string Record
    {
        get => (string)GetValue(RecordProperty);
        set => SetValue(RecordProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public LayoutOptions ContentHorizontalOptions
    {
        get => (LayoutOptions)GetValue(ContentHorizontalOptionsProperty);
        set => SetValue(ContentHorizontalOptionsProperty, value);
    }

    public LayoutOptions LogoHorizontalOptions
    {
        get => (LayoutOptions)GetValue(LogoHorizontalOptionsProperty);
        set => SetValue(LogoHorizontalOptionsProperty, value);
    }

    public TextAlignment ContentHorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(ContentHorizontalTextAlignmentProperty);
        set => SetValue(ContentHorizontalTextAlignmentProperty, value);
    }

    public int LogoColumn
    {
        get => (int)GetValue(LogoColumnProperty);
        set => SetValue(LogoColumnProperty, value);
    }

    public int ScoreColumn
    {
        get => (int)GetValue(ScoreColumnProperty);
        set => SetValue(ScoreColumnProperty, value);
    }
}