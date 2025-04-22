namespace Sports.Maui.Start.Pages.Sports;

using global::Sports.Maui.Model;
using global::Sports.Maui.Start.ViewModels.Sports;

[XamlCompilation(XamlCompilationOptions.Compile)]
public abstract partial class SportPage : BasePage
{
    public SportPage(SportViewModel viewModel)
    {
        InitializeComponent();
        SetViewModel(viewModel);
    }

    protected SportType SportType => GetViewModel<SportViewModel>().SportType;
}