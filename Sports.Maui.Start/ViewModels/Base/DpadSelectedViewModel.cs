namespace Sports.Maui.Start.ViewModels.Base;

using CommunityToolkit.Mvvm.Input;
using global::Sports.Maui.Start.Model;

public abstract class DpadSelectedViewModel<T> : DpadSelectedViewModel
    where T : DpadModel
{
    private int _hoveredIndex = -1;
    protected readonly Color _hoveredDefaultColor = Colors.OrangeRed;
    private SelectionMode _selectionMode = SelectionMode.Single;
    private T? _selectedItem = null;

    private int _previousHoveredIndex = -1;

    protected DpadSelectedViewModel()
    {
        SelectedItemCommand = new AsyncRelayCommand(OnItemSelected);
    }

    protected bool HasHovered { get; private set; }
    protected Func<CollectionView> GetCollectionView { get; private set; }

    protected abstract IList<T> Collection { get; }
    protected abstract Task OnItemSelected(T? selectedItem);
    public IAsyncRelayCommand SelectedItemCommand { get; set; }

    protected virtual Task OnNextIndex(int nextIndex)
    {
        return Task.CompletedTask;
    }

    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            NotifyPropertyChanged(nameof(SelectedItem));
        }
    }

    public SelectionMode SelectionMode
    {
        get => _selectionMode;
        set
        {
            _selectionMode = value;
            NotifyPropertyChanged(nameof(SelectionMode));
        }
    }

    public void SetCollectionView(CollectionView collectionView)
    {
        GetCollectionView = () => collectionView;
    }

    public async Task OnItemSelected()
    {
        if (SelectedItem == null)
        {
            return;
        }

        var hovered = GetCurrentHovered();
        if (hovered != null && hovered != SelectedItem)
        {
            SelectedItem = hovered;
            return;
        }

        await OnItemSelected(SelectedItem);
    }


    public override async Task OnDpadOkClicked()
    {
        await Task.CompletedTask;

        if (_hoveredIndex < 0)
        {
            await OnItemSelected(SelectedItem);
            return;
        }

        if (!HasHovered)
        {
            SelectedItem = Collection[_hoveredIndex];
            return;
        }

        SelectedItem = GetCurrentHovered();
    }

    protected async Task ResetHoveredIndex()
    {
        if (!HasHovered)
        {
            return;
        }

        HasHovered = false;
        _previousHoveredIndex = _hoveredIndex;
        _hoveredIndex = 0;
        await SetColors(item => item.DefaultColor);
    }

    protected T GetCurrentHovered()
    {
        if (!HasHovered)
        {
            return null;
        }

        if (_hoveredIndex < 0)
        {
            return null;
        }

        return Collection[_hoveredIndex];
    }

    public override async Task HoverOverNext(bool scrolledUp)
    {
        if (!HasHovered)
        {
            SelectedItem = null;
            HasHovered = true;
            return;
        }

        if (Collection.Count == 0)
        {
            return;
        }

        var maxIndex = Collection.Count - 1;
        if (maxIndex == 0)
        {
            _previousHoveredIndex = _hoveredIndex;
            _hoveredIndex = 0;
            await SetColors();
            return;
        }

        var currentIndex = _hoveredIndex;
        if (scrolledUp && currentIndex == 0)
        {
            _hoveredIndex = -1;
            await OnNextIndex(_hoveredIndex);
            ResetFirstItemHoveredColor();
            return;
        }

        var atMax = _hoveredIndex == maxIndex;

        if (!scrolledUp && atMax)
        {
            await OnNextIndex(maxIndex + 1);
            return;
        }

        _previousHoveredIndex = _hoveredIndex;
        _hoveredIndex = scrolledUp ? currentIndex - 1 : currentIndex + 1;

        await OnNextIndex(_hoveredIndex);

        await SetColors();
    }

    protected void TrySetCurrentHovered(int currentTry = 0, Func<T, Color> hoverOverride = null)
    {
        try
        {
            var item = Collection[_hoveredIndex];
            var hoverColor = hoverOverride != null ? hoverOverride(item) : _hoveredDefaultColor;
            item.HoveredColor = hoverColor;
        }
        catch
        {
        }
    }

    protected void ResetFirstItemHoveredColor()
    {
        var item = Collection[0];
        item.HoveredColor = item.DefaultColor;
    }

    private async Task SetColors(Func<T, Color> hoverOverride = null)
    {

        TrySetCurrentHovered(hoverOverride: hoverOverride);

        try
        {
            var previousItem = Collection[_previousHoveredIndex];
            previousItem.HoveredColor = previousItem.DefaultColor;
        }
        catch
        {
        }

        try
        {
            await MainThread.InvokeOnMainThreadAsync(() => GetCollectionView().ScrollTo(_hoveredIndex));
        }
        catch
        {
        }
    }
}

public abstract class DpadSelectedViewModel : BaseViewModel
{
    protected DpadSelectedViewModel()
    {
    }

    public abstract Task HoverOverNext(bool scrolledUp);
    public abstract Task OnDpadOkClicked();
}
