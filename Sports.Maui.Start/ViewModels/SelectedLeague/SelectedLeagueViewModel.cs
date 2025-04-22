namespace Sports.Maui.Start.ViewModels.SelectedLeague;

using CommunityToolkit.Mvvm.Input;
using global::Sports.Maui.Model;
using global::Sports.Maui.Model.Api;
using global::Sports.Maui.Start.Interface.Service;
using global::Sports.Maui.Start.Model.UI;
using global::Sports.Maui.Start.ViewModels.Base;
using System.Collections;
using System.Collections.ObjectModel;

public abstract class SelectedLeagueViewModel : SelectedLeagueBaseViewModel
{
    private readonly ObservableCollection<UIMatchGroup> _matchGroups;
    private readonly ObservableCollection<UIMatch> _collection;

    private bool _customTeamsSelected;
    private bool _customTeamsHovered;
    private Color _customTeamsHoveredColor;
    private bool _showCustomTeams;
    private bool _showSearch;
    private bool _searchEnabled = true;
    private string _searchText;

    public SelectedLeagueViewModel(
        ISelectedLeagueViewModelService selectedLeagueViewModelService)
            : base(selectedLeagueViewModelService)
    {
        _customTeamsHoveredColor = Colors.Transparent;

        _matchGroups = [];
        _collection = [];
        BackCommand = new AsyncRelayCommand(GoBack);
        RefreshCommand = new AsyncRelayCommand(OnRefresh);
        CustomTeamsSelectedCommand = new AsyncRelayCommand(OnCustomTeamsSelected);
        SearchCommand = new AsyncRelayCommand(OnSearchChange);
    }

    public void SetApiLeague(ApiLeague league)
    {
        ApiLeague = league;
    }

    public IAsyncRelayCommand CustomTeamsSelectedCommand { get; set; }
    public override IAsyncRelayCommand RefreshCommand { get; set; }
    public IAsyncRelayCommand BackCommand { get; set; }
    public IAsyncRelayCommand SearchCommand { get; set; }

    public bool ShowCustomTeams
    {
        get => _showCustomTeams;
        set
        {
            _showCustomTeams = value;
            NotifyPropertyChanged(nameof(ShowCustomTeams));
        }
    }

    public bool ShowSearch
    {
        get => _showSearch;
        set
        {
            _showSearch = value;
            NotifyPropertyChanged(nameof(ShowSearch));
        }
    }

    public bool SearchEnabled
    {
        get => _searchEnabled;
        set
        {
            _searchEnabled = value;
            NotifyPropertyChanged(nameof(SearchEnabled));
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            NotifyPropertyChanged(nameof(SearchText));
        }
    }

    public bool CollectionIsGrouped
    {
        // https://github.com/dotnet/maui/issues/18481
        get => DeviceInfo.Idiom != DeviceIdiom.TV && DeviceInfo.Platform != DevicePlatform.WinUI;
    }

    public bool CustomTeamsSelected
    {
        get => _customTeamsSelected;
        set
        {
            _customTeamsSelected = value;
            NotifyPropertyChanged(nameof(CustomTeamsSelected));
        }
    }

    public bool CustomTeamsHovered
    {
        get => _customTeamsHovered;
        set
        {
            _customTeamsHovered = value;
            NotifyPropertyChanged(nameof(CustomTeamsHovered));
        }
    }

    public Color CustomTeamsHoveredColor
    {
        get => _customTeamsHoveredColor;
        set
        {
            _customTeamsHoveredColor = value;
            NotifyPropertyChanged(nameof(CustomTeamsHoveredColor));
        }
    }

    public IEnumerable CollectionItemsSource
    {
        get => !CollectionIsGrouped ? _collection : _matchGroups;
    }

    protected override IList<UIMatch> Collection => _collection;

    protected abstract bool MatchesCustomTeams(UIMatch match);
    protected abstract Task OnCustomTeamsSelected(bool selected);

    public abstract LeagueType LeagueType { get; }

    public override async Task OnInitialPageLoad()
    {
        await Task.Run(async () =>
        {
            var uiLeague = await GetCurrentSeason();
            if (uiLeague == null)
            {
                ShowError(GetErrorMessage());
                return;
            }

            SetSelectedLeague(uiLeague);
            await SetSelectedLeagueMatches(uiLeague);

            if (!uiLeague.Matches.Any())
            {
                ShowError(GetErrorMessage());
                return;
            }


            var selectedPreference = LeaguePreferenceService.GetCustomTeamsPreference(ApiLeague);
            if (selectedPreference.HasValue && selectedPreference.Value)
            {
                await OnCustomTeamsSelected();
            }

            await UpdateUIMatches(uiLeague);
            ShowLoading = false;
            ShowMatches = true;
            ShowSearch = PagePreferenceService.GetShowSearchPref() ?? true;

            if (LeagueType != LeagueType.NFL)
            {
                ShowCustomTeams = true;
            }
        });
    }

    protected ApiLeague ApiLeague { get; private set; }

    protected override async Task OnNextIndex(int nextIndex)
    {
        var originalValue = SearchEnabled;
        try
        {
            SearchEnabled = false;
            if (nextIndex < 0)
            {
                _customTeamsHovered = true;
                CustomTeamsHoveredColor = _hoveredDefaultColor;
                originalValue = true;
                return;
            }

            if (!_customTeamsHovered)
            {
                originalValue = false;
                return;
            }

            _customTeamsHovered = false;
            CustomTeamsHoveredColor = Colors.Transparent;
            await Task.CompletedTask;
        }
        catch
        {

        }
        finally
        {
            SearchEnabled = originalValue;
        }
    }

    protected async Task OnCustomTeamsSelected()
    {
        _customTeamsSelected = !_customTeamsSelected;
        LeaguePreferenceService.SetCustomTeamsPreference(ApiLeague, _customTeamsSelected);

        await OnCustomTeamsSelected(_customTeamsSelected);

        await UpdateUIMatches(SelectedLeague);

        if (!_customTeamsSelected)
        {
            SelectedItem = null;
        }
    }

    protected async Task OnSearchChange()
    {
        try
        {
            await UpdateUIMatches(SelectedLeague!);
        }
        catch(Exception e) 
        {
        }
    }

    protected override async Task OnItemSelected(UIMatch? selectedMatch)
    {
        var searchEnabledForTV = ShowSearch && DeviceInfo.Idiom == DeviceIdiom.TV;

        if (_customTeamsHovered)
        {
            if (searchEnabledForTV)
            {
                SearchEnabled = true;
            }

            await OnCustomTeamsSelected();
            return;
        }

        if (searchEnabledForTV)
        {
            SearchEnabled = false;
        }

        if (selectedMatch!.IsFinished)
        {
            SelectedItem = null;
            return;
        }

        await base.OnItemSelected(selectedMatch);
    }

    protected async Task<UILeague?> GetCurrentSeason()
    {
        try
        {
            return await UiLeagueService.GetCurrentSeason(ApiLeague);
        }
        catch(Exception e)
        {
            return null;
        }
    }

    protected async Task SetSelectedLeagueMatches(UILeague league)
    {
        try
        {
            await UiLeagueService.SetMatches(league);
        }
        catch (Exception e)
        {
        }
    }

    protected async Task OnRefresh()
    {
        await Task.Run(async () =>
        {
            var updatedUILeague = await GetCurrentSeason();

            if (updatedUILeague == null)
            {
                IsRefreshing = false;
                ShowError(GetErrorMessage());
                return;
            }

            await SetSelectedLeagueMatches(updatedUILeague);
            if (!updatedUILeague.Matches.Any()) 
            {
                IsRefreshing = false;
                ShowError(GetErrorMessage());
                return;
            }

            SetSelectedLeague(updatedUILeague);

            await UpdateUIMatches(updatedUILeague);

            ErrorMessage = null;
            IsRefreshing = false;
        });

    }

    protected override async Task UpdateUIMatches(UILeague league)
    {
        try
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var searchMatches = league.Matches
                    .Where(MatchesSearch)
                    .ToList();

                UpdateMatchGroups(searchMatches);
                return;
            }

            if (!_customTeamsSelected)
            {
                UpdateMatchGroups(league.Matches);
                return;
            }

            var customTeamsMatches = league.Matches
                .Where(MatchesCustomTeams)
                .ToList();

            UpdateMatchGroups(customTeamsMatches);
        }
        catch { }

        await Task.CompletedTask;
    }

    protected void UpdateMatchGroups(IList<UIMatch> matches)
    {
        if (!CollectionIsGrouped)
        {
            ClearIfAny(_collection);

            var liveMatches = matches.Where(m => m.IsLive);
            var upcomingMatches = matches.Where(m => m.IsInFuture);
            var finishedMatches = matches.Where(m => m.IsFinished);
            foreach (var match in liveMatches)
            {
                _collection.Add(match);
            }
            foreach (var match in upcomingMatches)
            {
                _collection.Add(match);
            }
            foreach (var match in finishedMatches)
            {
                _collection.Add(match);
            }

            return;
        }

        ClearMatchGroups();
        var (liveGroup, upcomingGroup, finishedGroup) = GenerateNewGroups();

        SetMatchGroups
        (
            matches,
            liveGroup: liveGroup,
            upcomingGroup: upcomingGroup,
            finishedGroup: finishedGroup
        );
    }

    protected (UIMatchGroup Live, UIMatchGroup Upcoming, UIMatchGroup Finished)
        GenerateNewGroups()
    {
        var live = new UIMatchGroup("Live");
        var upcoming = new UIMatchGroup("Upcoming");
        var finished = new UIMatchGroup("Finished");

        return (live, upcoming, finished);
    }

    protected void SetMatchGroups(
        IList<UIMatch> matches,
        UIMatchGroup liveGroup,
        UIMatchGroup upcomingGroup,
        UIMatchGroup finishedGroup)
    {
        foreach (var match in matches)
        {
            if (match.IsLive)
            {
                liveGroup.Add(match);
                continue;
            }

            if (match.IsInFuture)
            {
                upcomingGroup.Add(match);
                continue;
            }

            finishedGroup.Add(match);
        }

        AddIfAny(liveGroup);
        AddIfAny(upcomingGroup);
        AddIfAny(finishedGroup);
    }

    protected void ClearMatchGroups()
    {
        ClearIfAny(_matchGroups);
    }

    protected void ClearIfAny<T>(IList<T> collection)
    {
        if (collection.Any())
        {
            collection.Clear();
        }
    }

    protected void AddIfAny(UIMatchGroup group)
    {
        if (!group.Any())
        {
            return;
        }

        _matchGroups.Add(group);
    }

    private string GetErrorMessage()
    {
        if(SelectedLeague == null)
        {
            return "Error loading league data";
        }

        if (SelectedLeague.ApiSeason == null || SelectedLeague.ApiSeason.IsOffSeason)
        {
            return "Off Season";
        }

        return "No games today/this week.";
    }

    private bool MatchesSearch(UIMatch match)
    {
        var firstTeamName = match.FirstTeam?.Name;
        var firstTeamLocation = match.FirstTeam?.Location;
        var secondTeamName = match.SecondTeam?.Name;
        var secondTeamLocation = match.SecondTeam?.Location;

        return !string.IsNullOrEmpty(firstTeamName) && firstTeamName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
            || !string.IsNullOrEmpty(firstTeamLocation) && firstTeamLocation.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
            || !string.IsNullOrEmpty(secondTeamName) && secondTeamName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
            || !string.IsNullOrEmpty(secondTeamLocation) && secondTeamLocation.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
            || !string.IsNullOrEmpty(match.GameTimeDisplay) && match.GameTimeDisplay.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase)
            || !string.IsNullOrEmpty(match.BroadcastStations) && match.BroadcastStations.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
    }
}
