using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace PharmaStoreInventory.Helpers;

public static class Alerts
{
    public static async Task DisplaySnackBar(string text, int duration = 10)
    {
        CancellationTokenSource cancellationTokenSource = new();

        var snackBarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.DarkBlue,
            TextColor = Colors.White,
            CornerRadius = new CornerRadius(10),
        };
        TimeSpan timeSpan = TimeSpan.FromSeconds(duration);

        var snackBar = Snackbar.Make(text, duration: timeSpan, visualOptions: snackBarOptions);

        await snackBar.Show(cancellationTokenSource.Token);
    }

    public static async Task DisplayToast(string text)
    {

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        ToastDuration duration = ToastDuration.Long;
        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }
}
