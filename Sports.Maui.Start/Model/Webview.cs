namespace Sports.Maui.Start.Model;

using Sports.Maui.Model.Interface;
using MauiWebview = WebView;

public class Webview : IWebview
{
    private MauiWebview _mauiWebview;

    public Webview(MauiWebview webView)
    {
        _mauiWebview = webView;
    }

    public async Task<string> EvaluateJavaScriptAsync(string script)
    {
        return await _mauiWebview.EvaluateJavaScriptAsync(script);
    }
}
