namespace Sports.Maui.Start.Platforms.Droid;
using Android.App;
using Android.Runtime;
using Sports.Maui.Start;

[Application]
public class MainApplication(nint handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}