namespace Sports.Maui.Start.ViewModels.Base;

using CommunityToolkit.Mvvm.Input;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;

public abstract class SelectedLeagueBaseViewModel : DpadSelectedViewModel<UIMatch>
{
    private bool _isRefreshing;
    protected readonly ISelectedLeagueViewModelService _selectedLeagueViewModelService;
    protected IUILeagueService UiLeagueService => _selectedLeagueViewModelService.UILeagueService;
    protected IServiceProvider ServiceProvider => _selectedLeagueViewModelService.ServiceProvider;
    protected ILeaguePreferenceService LeaguePreferenceService => _selectedLeagueViewModelService.LeaguePreferenceService;
    protected IPagePreferenceService PagePreferenceService => _selectedLeagueViewModelService.PagePreferenceService;

    private UILeague? _selectedLeague;
    private bool _showLoading = true;
    private bool _showMatches = false;
    private string? _errorMessage;

    public SelectedLeagueBaseViewModel(
        ISelectedLeagueViewModelService selectedLeagueViewModelService)
    {
        _selectedLeagueViewModelService = selectedLeagueViewModelService;
        LeagueNameCommand = new AsyncRelayCommand(OnLeagueNameTapped);
    }

    public UILeague? SelectedLeague
    {
        get => _selectedLeague;
        set
        {
            _selectedLeague = value;
            NotifyPropertyChanged(nameof(SelectedLeague));
        }
    }


    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            NotifyPropertyChanged(nameof(IsRefreshing));
        }
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

    public bool ShowMatches
    {
        get => _showMatches;
        set
        {
            _showMatches = value;
            NotifyPropertyChanged(nameof(ShowMatches));
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            NotifyPropertyChanged(nameof(ErrorMessage));
        }
    }

    public abstract IAsyncRelayCommand RefreshCommand { get; set; }
    public IAsyncRelayCommand LeagueNameCommand { get; set; }

    public void SetSelectedLeague(UILeague league)
    {
        SelectedLeague = league;
    }

    public override async Task OnPageAppearing()
    {
        await ResetHoveredIndex();
        SelectedItem = null;
        await Task.CompletedTask;
    }

    protected override async Task OnItemSelected(UIMatch? selectedMatch)
    {
        if (selectedMatch == null)
        {
            return;
        }
    }

    protected void ShowError(string errorMessage)
    {
        ErrorMessage = errorMessage;
        ShowLoading = false;
        ShowMatches = true;
    }

    protected Task OnLeagueNameTapped()
    {
        IsRefreshing = true;
        return Task.CompletedTask;
    }

    protected abstract Task UpdateUIMatches(UILeague league);
}