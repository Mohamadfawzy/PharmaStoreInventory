﻿using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class DashboardView : ContentPage, IRecipient<DashboardViewNotification>
{
    private readonly DashboardViewModel vm;

    #region OnStart
    public DashboardView()
    {
        InitializeComponent();
        vm = (DashboardViewModel)BindingContext;
        WeakReferenceMessenger.Default.Register<DashboardViewNotification>(this);
        double statusBarHeight = Application.Current?.MainPage?.Padding.Top ?? 0;
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        popup.HeightRequest = this.Height;
    }

    protected override bool OnBackButtonPressed()
    {
        if (vm.CanBackButtonPressed())
        {
            return base.OnBackButtonPressed();
        }
        return true;
    }

    public void Receive(DashboardViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.Display(message.Value);
        });
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var prefix = "مرحباً";
        userFullName.Text = $"{prefix} {AppPreferences.UserFullName}";
    }
    #endregion

    #region OnClicked
    private async void HamburgerTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SidebarView());
    }

    private async void GoToScanPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PickingView());
    }

    private async void GoToAllProductsPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AllStockView());
    }

    private async void BoxView_Loaded(object sender, EventArgs e)
    {
        var box = (BoxView)sender;
        while (placeholderElement.IsVisible)
        {
            await box.ScaleTo(0.98).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            await box.ScaleTo(1).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        }

        if (!placeholderElement.IsVisible)
        {
            box.Scale = 1;
        }
    }
    #endregion
}