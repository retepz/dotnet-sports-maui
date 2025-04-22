namespace Sports.Maui.Start.Platforms.Windows;

using Microsoft.Maui.Controls.Handlers.Items;

public static class WebviewHandler
{
    private static Microsoft.UI.Xaml.Controls.WebView2 _internalWebview;
    public static void Override()
    {
        //// https://github.com/dotnet/maui/issues/14557#issuecomment-1650647976
        //CollectionViewHandler.Mapper.AppendToMapping("HeaderAndFooterFix", (_, collectionView) =>
        //{
        //    collectionView.AddLogicalChild(collectionView.Header as Element);
        //});

        Microsoft.Maui.Handlers.WebViewHandler.Mapper.ModifyMapping(
            nameof(Microsoft.UI.Xaml.Controls.WebView2),
            (handler, view, args) =>
            {
                handler.PlatformView.CoreWebView2Initialized += (sender, initArgs) =>
                {
                    _internalWebview = sender;
                    sender.CoreWebView2.NewWindowRequested += (_sender, newWindowArgs) =>
                    {
                        newWindowArgs.Handled = true;
                    };
            };
        });
    }

    public static void Close()
    {
        try
        {
            _internalWebview.Close();
        }
        catch
        {

        }
    }
}
