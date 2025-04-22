namespace Sports.Maui.Start.Pages.TV;

using global::Sports.Maui.Start.ViewModels.TV;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TVChooseSportPage : BasePage
{
    public TVChooseSportPage(TVChooseSportViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetCollectionView(TVCollectionView.CollectionView);
        SetViewModel(viewModel);
    }
}