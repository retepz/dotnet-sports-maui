namespace Sports.Maui.Start.ViewModels.Sports;

using CommunityToolkit.Mvvm.Input;
using global::Sports.Maui.Model;
using global::Sports.Maui.Model.Api;
using global::Sports.Maui.Service.Interface.Api;
using global::Sports.Maui.Service.Interface.Cache;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using global::Sports.Maui.Start.Pages.SelectedLeague;
using global::Sports.Maui.Start.ViewModels.Base;
using System.Collections.ObjectModel;

public abstract class SportViewModel : BaseViewModel
{
    private readonly ObservableCollection<UISportLeague> _collection;
    private readonly IApiSportService _apiSportService;
    protected readonly IServiceProvider _serviceProvider;
    private readonly IPagePreferenceService _pagePreferenceService;
    private readonly IFileCacheService _fileCacheService;
    private UISportLeague? _selectedSportLeague;
    private ApiSport? _apiSport;

    private bool _showLoading = true;

    public SportViewModel(
        IApiSportService apiSportService,
        IServiceProvider serviceProvider,
        IPagePreferenceService pagePreferenceService,
        IFileCacheService fileCacheService)
    {
        _collection = [];
        SportLeagueSelectedCommand = new AsyncRelayCommand(OnSportLeagueSelected);
        _apiSportService = apiSportService;
        _serviceProvider = serviceProvider;
        _pagePreferenceService = pagePreferenceService;
        _fileCacheService = fileCacheService;
    }

    public abstract SportType SportType { get; }

    public virtual SelectedLeaguePage GetLeaguePage(UISportLeague uiSportLeague)
    {
        return _serviceProvider.GetRequiredService<GenericSelectedLeaguePage>();
    }

    public ObservableCollection<UISportLeague> SportLeagues
    {
        get => _collection;
    }

    public IAsyncRelayCommand SportLeagueSelectedCommand { get; set; }

    public UISportLeague? SelectedSportLeague
    {
        get => _selectedSportLeague;
        set
        {
            _selectedSportLeague = value;
            NotifyPropertyChanged(nameof(SelectedSportLeague));
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

    public override async Task OnPageAppearing()
    {
        _pagePreferenceService.DeleteLastVisitedLeagueId(_apiSport);

        SelectedSportLeague = null;
        ShowLoading = false;
        await Task.CompletedTask;
    }

    public override async Task OnInitialPageLoad()
    {
        await Task.Run(_fileCacheService.TryCleanupCache);

        _apiSport = await _apiSportService.Get(SportType);
        if(_apiSport == null)
        {
            ShowLoading = false;
            return;
        }

        var leagues = await _apiSportService.GetLeagues(_apiSport);

        if(leagues == null)
        {
            ShowLoading = false;
            return;
        }

        var lastVisitedLeagueId = _pagePreferenceService.GetLastVisitedLeagueId(_apiSport);

        UISportLeague lastVisitedLeague = null;

        foreach (var league in leagues)
        {
            var uiSportLeague = new UISportLeague(league);
            SportLeagues.Add(uiSportLeague);

            if(string.IsNullOrEmpty(lastVisitedLeagueId) || league.Id != lastVisitedLeagueId)
            {
                continue;
            }

            lastVisitedLeague = uiSportLeague;
        }

        if (lastVisitedLeague != null)
        {
            SelectedSportLeague = lastVisitedLeague;
            return;
        }

        ShowLoading = false;
    }

    protected async Task OnSportLeagueSelected() 
    {
        if(_apiSport == null || SelectedSportLeague == null)
        {
            return;
        }

        try
        {
            _pagePreferenceService.SetLastVisitedLeagueId(_apiSport, SelectedSportLeague.ApiLeague);

            var page = GetLeaguePage(SelectedSportLeague);
            page.SetApiLeague(SelectedSportLeague.ApiLeague);

            await NavigateTo(page);
        }
        catch
        {
            SelectedSportLeague = null;
        }
    }
}