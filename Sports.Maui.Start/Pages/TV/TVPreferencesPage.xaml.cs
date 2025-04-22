namespace Sports.Maui.Start.Pages.TV;

using global::Sports.Maui.Start.ViewModels.TV;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TVPreferencesPage : BasePage
{
    public TVPreferencesPage(TVPreferencesPageViewModel viewModel)
    {
        InitializeComponent();
        //viewModel.SetCollectionView(TVCollectionView.CollectionView);
        SetViewModel(viewModel);
    }
}