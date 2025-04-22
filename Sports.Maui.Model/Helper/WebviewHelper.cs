namespace Sports.Maui.Model.Helper;

using HtmlAgilityPack;
using Sports.Maui.Model.Interface;

public static class WebviewHelper
{
    public static async Task<HtmlDocument> GetHtmlDoc(IWebview view)
    {
        var html = await GetHtml(view);
        return DocHelper.GetFromHtml(html);
    }

    public static async Task<string> GetHtml(IWebview webview)
    {
        var encodedHtml = await webview.EvaluateJavaScriptAsync(
            "new XMLSerializer().serializeToString(document)");
        var html = System.Text.RegularExpressions.Regex.Unescape(encodedHtml);

        return html;
    }
}
