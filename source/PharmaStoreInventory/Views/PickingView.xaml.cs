using BarcodeScanning;
using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class PickingView : ContentPage, IRecipient<PickingViewNotification>
{
    readonly PickingViewModel viewModel;
    readonly bool IsScanMode = false;
    public PickingView()
    {
        Methods.AskForRequiredPermissionAsync();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<PickingViewNotification>(this);
        viewModel = new PickingViewModel();
        this.BindingContext = viewModel;
        IsScanMode = true;
    }

    public PickingView(string barcode)
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<PickingViewNotification>(this);
        viewModel = new PickingViewModel(barcode);
        this.BindingContext = viewModel;
    }
    protected override bool OnBackButtonPressed()
    {
        if (viewModel.IsEditPopupVisible)
        {
            viewModel.IsEditPopupVisible = false;
            return true;
        }
        return base.OnBackButtonPressed();

    }
    public void Receive(PickingViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.ShowMessage(message.Value);
        });
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (IsScanMode)
        {
            guidesLines.IsVisible = true;
            nativeBarcode.IsVisible = true;
            nativeBarcode.CameraEnabled = true;
            nativeBarcode.PauseScanning = false;
            mainContianer.SetRow(gridData, 2);
        }
        //  is it shown in data mode? comes from AllStockView
        else
        {
            gridData.IsVisible = true;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (AppValues.LeftScanIcon)
            {
                header.FlowDirection = FlowDirection.RightToLeft;
            }
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar("OnAppearing" + ex.Message);
        }

    }

    private async void CameraOnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        await Dispatcher.DispatchAsync(() =>
        {
            if (e.BarcodeResults.Length <= 0)
                return;
            gridData.IsVisible = true;
            mainContianer.SetRow(gridData, 1);
            nativeBarcode.PauseScanning = true;
            nativeBarcode.CameraEnabled = false;
            Task.Run(() => { viewModel.FetchStockDetails(e.BarcodeResults[0].DisplayValue); });
            //viewModel.FetchStockDetails(e.BarcodeResults[0].DisplayValue);
            // Dispatcher.DispatchAsync(() => { });

        });
    }

    private void NewScanTapped(object sender, TappedEventArgs e)
    {
        nativeBarcode.IsVisible = true;
        nativeBarcode.PauseScanning = false;
        nativeBarcode.CameraEnabled = true;
        mainContianer.SetRow(gridData, 2);
    }

    private void CloseCameraTapped(object sender, TappedEventArgs e)
    {
        nativeBarcode.IsVisible = false;
        nativeBarcode.PauseScanning = true;
        nativeBarcode.CameraEnabled = false;
        mainContianer.SetRow(gridData, 1);
    }

    private void ToggleFlashLight(object sender, TappedEventArgs e)
    {
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
    }

    private void OnDoneSwipeItemInvoked(object sender, EventArgs e)
    {

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

    void ClosePopup()
    {
        viewModel.IsEditPopupVisible = false;
        //entry.HideSoftInputAsync(CancellationToken.None);
    }

    private void Popup_HideSoftInput_Tapped(object sender, TappedEventArgs e)
    {
        //entry.HideSoftInputAsync(CancellationToken.None);
        //yearMonthDayStack.
    }

    private void ExecuteSelectionChanged_Tapped(object sender, TappedEventArgs e)
    {

        if (e.Parameter != null)
        {
            var param = e.Parameter as ProductDetailsModel;
            if (param != null)
            {
                viewModel.ExecuteSelectionChanged(param);
            }
        }
    }

    //Deleted

    //private void TapToScan(object sender, TappedEventArgs e)
    //{
    //    //nativeBarcode.PauseScanning = false;
    //    //nativeBarcode.TapToFocusEnabled = true;
    //}


    //private void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (collection.SelectedItem == null) return;
    //    if (collection.SelectedItem != null)
    //    {
    //        //popup.IsVisible = true;
    //        //backgroundTransparence.IsVisible = true;
    //    }
    //    collection.SelectedItem = null;
    //}

    //private void Save_Clicked(object sender, EventArgs e)
    //{
    //    ClosePopup();
    //}


    //private void OpenPopupTapped(object sender, TappedEventArgs e)
    //{
    //    OpenPopup();
    //}


    //void OpenPopup()
    //{
    //    popup.IsVisible = true;
    //    backgroundTransparence.IsVisible = true;
    //    nativeBarcode.PauseScanning = true;
    //}

}