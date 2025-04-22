namespace Sports.Maui.Start.ViewModels.TV;

using global::Sports.Maui.Start.Model;
using global::Sports.Maui.Start.ViewModels.Base;

public abstract class TVBasePageViewModel<T>() : DpadSelectedViewModel<T>
    where T: DpadModel
{
    private bool _showLoading = true;
    private int _opacity = 0;

    public sealed override async Task OnPageAppearing()
    {
        await ResetHoveredIndex();
        SelectedItem = null;
        ShowLoading = false;
        Opacity = 1;

        await OnAppearing();
    }

    public bool ShowLoading
    {
        get => _showLoading;
        set
        {
            _showLoading = value;
            NotifyPropertyChanged(nameof(ShowLoading));
        }
    }

    public int Opacity
    {
        get => _opacity;
        set
        {
            _opacity = value;
            NotifyPropertyChanged(nameof(Opacity));
        }
    }

    protected abstract Task OnSelected(T? item);
    protected abstract Task OnAppearing();

    protected sealed override async Task OnItemSelected(T? item)
    {
        if(item == null)
        {
            SelectedItem = null;
            return;
        }

        Opacity = 0;
    
        await OnSelected(item);
    }
}