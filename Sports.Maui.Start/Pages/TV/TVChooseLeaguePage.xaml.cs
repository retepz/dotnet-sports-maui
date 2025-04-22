namespace Sports.Maui.Start.Pages.TV;

using global::Sports.Maui.Model.Api;
using global::Sports.Maui.Start.ViewModels.TV;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TVChooseLeaguePage : BasePage
{
    public TVChooseLeaguePage(TVChooseLeagueViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetCollectionView(TVCollectionView.CollectionView);
        SetViewModel(viewModel);
    }

    public void SetApiSport(ApiSport apiSport)
    {
        GetViewModel<TVChooseLeagueViewModel>().SetApiSport(apiSport);
    }
}