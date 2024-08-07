﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Helpers;

public static class Alerts
{
    public static async Task DisplayAlert(string message)
    {
        await Application.Current?.MainPage?.DisplayAlert("Exception", message, "cancel");
    }


    public static async Task DisplaySnackbar(string text, int durationS = 3)
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
        TimeSpan duration = TimeSpan.FromSeconds(durationS);

        var snackbar = Snackbar.Make(text, visualOptions: snackbarOptions, duration: duration, actionButtonText: "");

        await snackbar.Show(cancellationTokenSource.Token);
    }

    private static Popup popup;
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
