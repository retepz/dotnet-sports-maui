namespace Sports.Maui.Start.Views;

using System.Collections;
using System.Windows.Input;

public partial class TVCollectionView : ContentView
{
    public static readonly BindableProperty ShowLoadingProperty =
        BindableProperty.Create(nameof(ShowLoading), typeof(bool), typeof(TVCollectionView));

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(TVCollectionView));

    public static readonly BindableProperty CollectionViewOpacityProperty =
        BindableProperty.Create(nameof(CollectionViewOpacity), typeof(int), typeof(TVCollectionView));

    public static readonly BindableProperty SelectionModeProperty =
        BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(TVCollectionView),
            SelectionMode.None);

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TVCollectionView), default,
            defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty SelectionChangedCommandProperty =
        BindableProperty.Create(nameof(SelectionChangedCommand), typeof(ICommand), typeof(TVCollectionView));

    public static readonly BindableProperty HeaderImageProperty =
        BindableProperty.Create(nameof(HeaderImage), typeof(string), typeof(TVCollectionView));

    public static readonly BindableProperty HeaderLabelProperty =
        BindableProperty.Create(nameof(HeaderLabel), typeof(string), typeof(TVCollectionView));

    public static readonly BindableProperty HeaderIsVisibleProperty =
        BindableProperty.Create(nameof(HeaderIsVisible), typeof(bool), typeof(TVCollectionView));

    public TVCollectionView()
	{
		InitializeComponent();
	}

    public bool ShowLoading
    {
        get => (bool)GetValue(ShowLoadingProperty);
        set => SetValue(ShowLoadingProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public int CollectionViewOpacity
    {
        get => (int)GetValue(CollectionViewOpacityProperty);
        set => SetValue(CollectionViewOpacityProperty, value);
    }

    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }

    public SelectionMode SelectionMode
    {
        get => (SelectionMode)GetValue(SelectionModeProperty);
        set => SetValue(SelectionModeProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public string HeaderImage
    {
        get => (string)GetValue(HeaderImageProperty);
        set => SetValue(HeaderImageProperty, value);
    }

    public string HeaderLabel
    {
        get => (string)GetValue(HeaderLabelProperty);
        set => SetValue(HeaderLabelProperty, value);
    }

    public bool HeaderIsVisible
    {
        get => (bool)GetValue(HeaderIsVisibleProperty);
        set => SetValue(HeaderIsVisibleProperty, value);
    }

    public CollectionView CollectionView => Collection;
}