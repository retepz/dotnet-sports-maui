namespace Sports.Maui.Start.Pages;

using global::Sports.Maui.Start.ViewModels.Base;

public abstract class BasePage : ContentPage, IDisposable
{
    private bool _finishedInitialLoad;
    private bool disposedValue;

    protected BasePage()
    {
        NavigationPage.SetHasNavigationBar(this, false);
    }

    public T GetViewModel<T>()
        where T : class
    {
        return BindingContext as T;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected void SetViewModel<T>(T viewModel)
    {
        BindingContext = viewModel;
        SetGetPage();
    }

    protected override sealed bool OnBackButtonPressed()
    {
        Dispose();
        var handled = GetViewModel<BaseViewModel>().OnBackPressed();

        return handled || base.OnBackButtonPressed();
    }

    protected override sealed void OnAppearing()
    {
        var viewModel = GetViewModel<BaseViewModel>();
        if (!_finishedInitialLoad)
        {
            _finishedInitialLoad = true;
            viewModel.OnInitialPageLoad();
            return;
        }
        viewModel.OnPageAppearing();
    }

    protected override sealed void OnDisappearing()
    {
        GetViewModel<BaseViewModel>().OnPageDisappearing();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                GetViewModel<BaseViewModel>().Dispose();
            }

            disposedValue = true;
        }
    }

    private void SetGetPage()
    {
        var viewModel = GetViewModel<BaseViewModel>();
        viewModel.SetGetPage(() => this);
    }
}