namespace Sports.Maui.Start.ViewModels.Base;

using global::Sports.Maui.Start.Model;
using global::Sports.Maui.Start.Pages;

public abstract class BaseViewModel : NotifyPropertyChangedModel, IDisposable
{
    protected Func<BasePage> _getPage;
    private bool _disposedValue;

    protected BaseViewModel()
    {
    }

    public void SetGetPage(Func<BasePage> getPage)
    {
        _getPage = getPage;
    }

    protected virtual async Task NavigateTo(Page page)
    {
        if (Shell.Current != null)
        {
            await ShellNavigateTo(page);
            return;
        }

        var currentPage = _getPage();
        await StandardNavigateTo(page, currentPage);
    }

    protected async Task DisplayAsModal(Page page)
    {
        if (Shell.Current != null)
        {
            await ShellDisplayAsModal(page);
            return;
        }

        await StandardDisplayAsModal(page);
    }

    protected virtual async Task GoBack()
    {
        var currentPage = _getPage();

        if (Shell.Current != null && Shell.Current.Navigation.ModalStack.Count > 0)
        {
            await ShellPopModal();
        }
        else if (Shell.Current != null)
        {
            await ShellPop();
        }
        else if (currentPage.Navigation.ModalStack.Count > 0)
        {
            await StandardPopModal(currentPage);
        }
        else
        {
            await StandardPop(currentPage);
        }

        currentPage.Dispose();
    }

    private static async Task StandardPop(BasePage currentPage)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await currentPage.Navigation.PopAsync(true);
        });
    }

    public virtual async Task OnInitialPageLoad()
    {
        await OnPageAppearing();
    }

    public abstract Task OnPageAppearing();

    public virtual async Task OnPageDisappearing()
    {
        await Task.CompletedTask;
    }

    public virtual bool OnBackPressed()
    {
        return false;
    }

    protected virtual void InternalDispose()
    {

    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~BaseViewModel()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                InternalDispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    private static async Task ShellNavigateTo(Page page)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PushAsync(page);
        });
    }

    private static async Task StandardNavigateTo(Page page, BasePage currentPage)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await currentPage.Navigation.PushAsync(page);
        });
    }

    private async Task StandardDisplayAsModal(Page page)
    {
        var currentPage = _getPage();
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await currentPage.Navigation.PushModalAsync(page, animated: true);
        });
    }

    private static async Task ShellDisplayAsModal(Page page)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PushModalAsync(page, animated: true);
        });
    }

    private static async Task StandardPopModal(BasePage currentPage)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await currentPage.Navigation.PopModalAsync(true);
        });
    }

    private static async Task ShellPop()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PopAsync(true);
        });
    }

    private static async Task ShellPopModal()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Shell.Current.Navigation.PopModalAsync(true);
        });
    }
}