using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Helpers;

public static class Alerts
{
    public static async Task DisplaySnackbar(string text, int durationS = 10)
    {

        CancellationTokenSource cancellationTokenSource = new();

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
        TimeSpan duration = TimeSpan.FromSeconds(durationS);

        var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);

        await snackbar.Show(cancellationTokenSource.Token);
    }

    public static void SendNotification(ErrorMessage errorMessage)
    {
        WeakReferenceMessenger.Default.Send(new NotificationMessage(errorMessage));
    }

    private static Popup? popup;
    public static async void DisplayActivityIndicator(Page page)
    {
        popup = new PopupWin();
        await page.ShowPopupAsync(popup);
    }

    public static async void CloseActivityIndicator()
    {
        if (popup != null)
        {
            await popup.CloseAsync();
        }
    }


}
