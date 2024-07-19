using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace PharmaStoreInventory.Helpers;

public static class CatchingException
{
    public static async Task PharmaDisplayAlert(string message)
    {
        await Application.Current?.MainPage?.DisplayAlert("Exception", message, "cancel");
    }


    public static async void DisplaySnackbar(string text)
    {

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.DarkBlue,
            TextColor = Colors.White,
            //ActionButtonTextColor = Colors.Yellow,
            // ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
            CornerRadius = new CornerRadius(10),
            //Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            //CharacterSpacing = 0.5
        };

        //string text = "The branch has been contacted successfully";
        //string actionButtonText = "Click Here to Dismiss";
        //Action action = async () => await DisplayAlert("Snackbar ActionButton Tapped", "The user has tapped the Snackbar ActionButton", "OK");
        TimeSpan duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(text, visualOptions: snackbarOptions,actionButtonText:"");

        await snackbar.Show(cancellationTokenSource.Token);
    }
}
