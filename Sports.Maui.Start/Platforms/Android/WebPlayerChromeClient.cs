namespace Sports.Maui.Start.Platforms.Droid;

using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using View = Android.Views.View;

public class WebPlayerChromeClient : MauiWebChromeClient
{
    private WeakReference<Android.App.Activity> _activityRef;
    private View _customView;
    private ICustomViewCallback _videoViewCallback;
    bool _isSystemBarVisible;
    int _defaultSystemUiVisibility;

    public WebPlayerChromeClient(IWebViewHandler handler) : base(handler)
    {
        _activityRef = new(Platform.CurrentActivity);
    }

    // OnShowCustomView operate the perform call back to video view functionality
    // is visible in the view.
    public override void OnShowCustomView(View? view, ICustomViewCallback? callback)
    {
        if (_customView is not null)
        {
            OnHideCustomView();
            return;
        }

        _activityRef.SetTarget(Platform.CurrentActivity);
        _activityRef.TryGetTarget(out var context);
        if (context is null)
            return;

        _videoViewCallback = callback;

        _customView = view;
        _customView.SetBackgroundColor(Android.Graphics.Color.White);

        context.RequestedOrientation = Android.Content.PM.ScreenOrientation.Sensor;

        // Hide the SystemBars and Status bar
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            context.Window.SetDecorFitsSystemWindows(false);

            var windowInsets = context.Window.DecorView.RootWindowInsets;
            _isSystemBarVisible = windowInsets.IsVisible(WindowInsetsCompat.Type.NavigationBars()) || windowInsets.IsVisible(WindowInsetsCompat.Type.StatusBars());

            if (_isSystemBarVisible)
                context.Window.InsetsController?.Hide(WindowInsets.Type.SystemBars());
        }
        else
        {
#pragma warning disable CS0618 // Type or member is obsolete
            _defaultSystemUiVisibility = (int)context.Window.DecorView.SystemUiVisibility;
            int systemUiVisibility = _defaultSystemUiVisibility | (int)SystemUiFlags.LayoutStable | (int)SystemUiFlags.LayoutHideNavigation | (int)SystemUiFlags.LayoutHideNavigation |
                (int)SystemUiFlags.LayoutFullscreen | (int)SystemUiFlags.HideNavigation | (int)SystemUiFlags.Fullscreen | (int)SystemUiFlags.Immersive;
            context.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)systemUiVisibility;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        // Add the CustomView
        if (context.Window.DecorView is FrameLayout layout)
            layout.AddView(_customView, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));
    }

    // OnHideCustomView is the WebView call back when the load view in full screen
    // and hide the custom container view.
    public override void OnHideCustomView()
    {
        _activityRef.TryGetTarget(out var context);

        if (context is null)
            return;

        // Remove the CustomView
        if (context.Window.DecorView is FrameLayout layout)
            layout.RemoveView(_customView);

        // Show again the SystemBars and Status bar
        if (OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            context.Window.SetDecorFitsSystemWindows(true);

            if (_isSystemBarVisible)
                context.Window.InsetsController?.Show(WindowInsets.Type.SystemBars());
        }
        else
#pragma warning disable CS0618 // Type or member is obsolete
            context.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)_defaultSystemUiVisibility;
#pragma warning restore CS0618 // Type or member is obsolete

        _videoViewCallback.OnCustomViewHidden();
        _customView = null;
        _videoViewCallback = null;
    }
}