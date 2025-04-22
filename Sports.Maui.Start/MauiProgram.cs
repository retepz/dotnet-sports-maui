namespace Sports.Maui.Start;

using Sports.Maui.Start.Pages.TV;
using Sports.Maui.Service.Cache;
using Sports.Maui.Service.Config;
using Sports.Maui.Service.Interface.Cache;
using Sports.Maui.Service.Interface;
using Sports.Maui.Service;
using Sports.Maui.Start.Interface.Service;
using Sports.Maui.Start.Service;
using Sports.Maui.Start.ViewModels.TV;
using Sports.Maui.Start.ViewModels.SelectedLeague;
using Sports.Maui.Start.ViewModels.Sports;
using Sports.Maui.Start.Pages.SelectedLeague;
using Sports.Maui.Start.Pages.Sports;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using CommunityToolkit.Maui;
using Sports.Maui.Service.Api;
using Sports.Maui.Service.Interface.Api;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCompatibility()
            .UseMauiCommunityToolkit();

        builder
            .Services
            .AddMemoryCache();

        builder.Services.AddSingleton(typeof(IApiService<>), typeof(ApiService<>));
        builder.Services.AddSingleton(typeof(IApiCacheItemService<>), typeof(ApiCacheItemService<>));
        builder.Services.AddSingleton<IApiCompetitionService, ApiCompetitionService>();
        builder.Services.AddSingleton<IApiCompetitorsService, ApiCompetitorsService>();
        builder.Services.AddSingleton<IApiEventService, ApiEventService>();
        builder.Services.AddSingleton<IApiLeagueService, ApiLeagueService>();
        builder.Services.AddSingleton<IApiSportService, ApiSportService>();
        builder.Services.AddSingleton<IApiTeamService, ApiTeamService>();
        builder.Services.AddSingleton<IApiWeekEventService, ApiWeekEventService>();
        builder.Services.AddSingleton<IApiWeekService, ApiWeekService>();
        builder.Services.AddSingleton<IPagePreferenceService, PagePreferenceService>();
        builder.Services.AddSingleton<IUserPreferenceService, UserPreferenceService>();
        builder.Services.AddSingleton<ILeaguePreferenceService, LeaguePreferenceService>();

        builder.Services.AddSingleton<IUILeagueService, UILeagueService>();
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<IFileCacheService, FileCacheService>();
        builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

        builder.Services.AddTransient<TVPreferencesPageViewModel>();
        builder.Services.AddTransient<TVChooseLeagueViewModel>();
        builder.Services.AddTransient<TVChooseSportViewModel>();
        builder.Services.AddTransient<NflSelectedLeagueViewModel>();
        builder.Services.AddTransient<GenericSelectedLeagueViewModel>();
        builder.Services.AddTransient<ISelectedLeagueViewModelService, SelectedLeagueViewModelService>();

        builder.Services.AddTransient<FootballViewModel>();
        builder.Services.AddTransient<HockeyViewModel>();
        builder.Services.AddTransient<BasketballViewModel>();
        builder.Services.AddTransient<BaseballViewModel>();

        builder.Services.AddTransient<TVPreferencesPage>();
        builder.Services.AddTransient<TVChooseLeaguePage>();
        builder.Services.AddTransient<TVChooseSportPage>();
        builder.Services.AddTransient<NflSelectedLeaguePage>();
        builder.Services.AddTransient<GenericSelectedLeaguePage>();

        builder.Services.AddTransient<FootballPage>();
        builder.Services.AddTransient<HockeyPage>();
        builder.Services.AddTransient<BasketballPage>();
        builder.Services.AddTransient<BaseballPage>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.Configure<FileSystemConfig>(configOptions =>
        {
            configOptions.CacheDirectory = FileSystem.CacheDirectory;
        });

        PlatformSpecificSetup();

        return builder.Build();
    }

    private static void PlatformSpecificSetup()
    {
#if WINDOWS
    Sports.Maui.Start.Platforms.Windows.WebviewHandler.Override();
#endif

#if ANDROID
        Sports.Maui.Start.Platforms.Droid.CustomWebviewHandler.EnableVideoFeatures();
        Sports.Maui.Start.Platforms.Droid.CustomCollectionViewHandler.Override();
#endif
    }
}
