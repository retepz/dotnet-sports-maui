namespace Sports.Maui.Start.Platforms.Droid;
using Android.App;
using Android.Content.PM;
using AndroidX.AppCompat.App;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.UiMode | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public MainActivity()
    {
        AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightYes;
    }
}