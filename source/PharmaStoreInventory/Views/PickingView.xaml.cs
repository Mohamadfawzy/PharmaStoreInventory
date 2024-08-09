using BarcodeScanning;
using CommunityToolkit.Maui.Core.Platform;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class PickingView : ContentPage
{
    readonly PickingViewModel viewModel;
    public PickingView()
    {
        this.FlowDirection = FlowDirection.RightToLeft;
        InitializeComponent();
        viewModel = (PickingViewModel)BindingContext;
        Methods.AskForRequiredPermissionAsync();
    }
    protected override void OnAppearing()
    {

    }

    private void CameraOnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        var first = e.BarcodeResults.FirstOrDefault();
        if (first is null)
            return;

        Dispatcher.DispatchAsync(() =>
        {
            viewModel.GetStockDetails();

        });
        mainContianer.SetRow(gridData,1);
        nativeBarcode.PauseScanning = true;
        nativeBarcode.CameraEnabled=false;
    }
    private void NewScanTapped(object sender, TappedEventArgs e)
    {
        nativeBarcode.PauseScanning = false;
        nativeBarcode.CameraEnabled = true;
        mainContianer.SetRow(gridData, 2);
    }

    private void ToggleFlashLight(object sender, TappedEventArgs e)
    {
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
    }

    private void TapToScan(object sender, TappedEventArgs e)
    {
        nativeBarcode.PauseScanning = false;
    }

    private void OnDoneSwipeItemInvoked(object sender, EventArgs e)
    {
        //DisplayAlert("title", "massage", "cancel");
    }

    private async void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {

        if (e.SwipeDirection == SwipeDirection.Right)
        {
            var swip = sender as SwipeView;
            //var first = swip?.LeftItems.First() as SwipeItemView;
            if (swip?.FindByName("checkIcon") is Label checkIcon)
            {
                await checkIcon.TranslateTo(0, 10);
                await checkIcon.ScaleTo(1.2, 400);
                await checkIcon.ScaleTo(0.8, 400);
                await checkIcon.ScaleTo(1, 400);
                await checkIcon.TranslateTo(0, 0);
            }

        }
    }

    private void ClosePopupTapped(object sender, TappedEventArgs e)
    {
        ClosePopup();
    }

    private void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (collection.SelectedItem == null) return;
        if (collection.SelectedItem != null)
        {
            //popup.IsVisible = true;
            //backgroundTransparence.IsVisible = true;
        }
        collection.SelectedItem = null;
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        ClosePopup();
    }


    private void OpenPopupTapped(object sender, TappedEventArgs e)
    {
        OpenPopup();
    }

    void ClosePopup()
    {
        popup.IsVisible = false;
        backgroundTransparence.IsVisible = false;
        nativeBarcode.PauseScanning = false;
        entry.HideSoftInputAsync(CancellationToken.None);
    }

    void OpenPopup()
    {
        popup.IsVisible = true;
        backgroundTransparence.IsVisible = true;
        nativeBarcode.PauseScanning = true;
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {

    }
}