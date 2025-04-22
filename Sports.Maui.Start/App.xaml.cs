namespace Sports.Maui.Start;

using Sports.Maui.Start.Pages.TV;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Dark;
        _serviceProvider = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var defaultPage = GetDefaultPage(_serviceProvider);
        return new Window(defaultPage);
    }

    protected static Page GetDefaultPage(IServiceProvider serviceProvider)
    {
        return DeviceInfo.Idiom == DeviceIdiom.TV
        ? GetNavPage(serviceProvider)
        : GetAppShell(serviceProvider);
    }

    private static AppShell GetAppShell(IServiceProvider serviceProvider)
    {
        var shell = serviceProvider.GetRequiredService<AppShell>();
        return shell;
    }

    private static NavigationPage GetNavPage(IServiceProvider serviceProvider)
    {
        var mainPage = serviceProvider.GetRequiredService<TVChooseSportPage>();

        return new NavigationPage(mainPage);
    }
}