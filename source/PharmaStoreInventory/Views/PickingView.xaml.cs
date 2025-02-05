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
    private bool IsScanMode = false;

    #region OnStart
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
            notification.Display(message.Value);
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
        DeviceDisplay.Current.KeepScreenOn = true;
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
            await Alerts.DisplaySnackBar("OnAppearing" + ex.Message);
        }

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        DeviceDisplay.Current.KeepScreenOn = false;
    }
    #endregion

    #region OnClicked

    private async void CameraOnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        await Dispatcher.DispatchAsync(() =>
        {
            if (e.BarcodeResults.Length <= 0)
                return;
            ToggleCameraVisibility();
            Task.Run(() => { viewModel.FetchStockDetails(e.BarcodeResults[0].DisplayValue); });
        });
    }

    private void ToggleCameraVisibilityTapped(object sender, TappedEventArgs e)
    {
        ToggleCameraVisibility();
    }

    private void ToggleCameraVisibility()
    {
        IsScanMode = !IsScanMode;
        nativeBarcode.IsVisible = !nativeBarcode.IsVisible;
        nativeBarcode.PauseScanning = !nativeBarcode.PauseScanning;
        nativeBarcode.CameraEnabled = !nativeBarcode.CameraEnabled;
        mainContianer.SetRow(gridData, IsScanMode ? 2 : 1);
    }

    private void ToggleFlashLight(object sender, TappedEventArgs e)
    {
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
    }

    private async void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        if (e.SwipeDirection == SwipeDirection.Right)
        {
            var swap = sender as SwipeView;
            if (swap?.FindByName("checkIcon") is Label checkIcon)
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

    private void Popup_HideSoftInput_Tapped(object sender, TappedEventArgs e)
    {
        //entry.HideSoftInputAsync(CancellationToken.None);
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
    #endregion

    #region On process
    void ClosePopup()
    {
        viewModel.IsEditPopupVisible = false;
    }
    #endregion
}



//private void NewScanTapped(object sender, TappedEventArgs e)
//{
//    nativeBarcode.IsVisible = true;
//    nativeBarcode.PauseScanning = false;
//    nativeBarcode.CameraEnabled = true;
//    mainContianer.SetRow(gridData, 2);
//}
//private void CloseCameraTapped(object sender, TappedEventArgs e)
//{
//    nativeBarcode.IsVisible = false;
//    nativeBarcode.PauseScanning = true;
//    nativeBarcode.CameraEnabled = false;
//    mainContianer.SetRow(gridData, 1);
//}