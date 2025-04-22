namespace Sports.Maui.Start.Platforms.Droid;
public class CustomWebviewHandler
{
    private static Android.Webkit.WebView? _webview;
    private static WebPlayerChromeClient? _chromeClient;
    public static void EnableVideoFeatures()
    {
        Microsoft.Maui.Handlers.WebViewHandler.Mapper.ModifyMapping(
        nameof(Android.Webkit.WebView.WebChromeClient),
        (handler, view, args) =>
        {
            _chromeClient = new WebPlayerChromeClient(handler);
            handler.PlatformView.SetWebChromeClient(_chromeClient);
            _webview = handler.PlatformView;
        });
    }

    public static async Task StandardVideoFullscreen()
    {
        if (_webview == null)
        {
            return;
        }

        const int clickCount = 2;

        for (int i = 0; i < clickCount; i++)
        {
            var clickVideo = @$"javascript:(function standardVideoClick() {{ document.querySelector('video').click(); }})()";
            _webview.LoadUrl(clickVideo);
            await Task.Delay(500);
        }

        var playVideo = @$"javascript:(function standardVideoPlay() {{ document.querySelector('video').play(); }})()";
        _webview.LoadUrl(playVideo);
        await Task.Delay(500);

        var requestFullscreen = @$"javascript:(function standardVideoFullscreen() {{ document.querySelector('video').requestFullscreen(); }})()";
        _webview.LoadUrl(requestFullscreen);
        await Task.Delay(1000);

        var volumeVideo = @$"javascript:(function standardVideoVolume() {{ document.querySelector('video').volume = 1; }})()";
        _webview.LoadUrl(volumeVideo);
    }

    public static async Task CloseFullscreenVideo()
    {
        if (_webview == null)
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            var exitFullscreen = @$"javascript:(function exitFullscreen() {{ document.exitFullscreen(); }})()";
            _webview.LoadUrl(exitFullscreen);
        });
    }
}
