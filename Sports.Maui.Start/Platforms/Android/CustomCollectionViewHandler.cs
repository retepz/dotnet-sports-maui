namespace Sports.Maui.Start.Platforms.Droid;
using Android.Views;
using Sports.Maui.Start.Pages;
using Sports.Maui.Start.ViewModels.Base;
using PlatformView = Android.Views.View;

public class CustomCollectionViewHandler
{
    private static readonly Keycode[] _handledCodes =
    [
        Keycode.DpadUp,
        Keycode.DpadDown,
        Keycode.DpadCenter,
        Keycode.ButtonSelect
    ];

    public static void Override()
    {
        Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.ModifyMapping(
        nameof(PlatformView),
        (handler, view, args) =>
        {
            var currentPage = Application.Current!.Windows![0].Page;
            if (currentPage is not NavigationPage navPage)
            {
                return;
            }

            if (navPage.CurrentPage is not BasePage page)
            {
                return;
            }

            var dpad = page.GetViewModel<DpadSelectedViewModel>();
            if (dpad == null)
            {
                return;
            }

            handler.PlatformView.UnhandledKeyEvent += Handle(dpad);
        });
    }

    private static EventHandler<PlatformView.UnhandledKeyEventEventArgs> Handle(DpadSelectedViewModel dpad)
    {
        return (sender, e) =>
        {
            if (e.Event?.Action != KeyEventActions.Down)
            {
                e.Handled = false;
                return;
            }

            var keyCode = e.Event.KeyCode;
            var isHandled = _handledCodes.Contains(keyCode);
            if (!isHandled)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;

            try
            {
                if (keyCode == Keycode.DpadCenter || keyCode == Keycode.ButtonSelect)
                {
                    dpad.OnDpadOkClicked().Wait();
                    return;
                }

                var nextDirectionIsUp = keyCode == Keycode.DpadUp;
                dpad.HoverOverNext(nextDirectionIsUp).Wait();
            }
            catch
            {

            }
        };
    }
}
