using BarcodeScanning;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class PickingView : ContentPage
{
    readonly PickingViewModel viewModel;
    bool IsScanMode = false;
    public PickingView()
    {
        InitializeComponent();
        viewModel = new PickingViewModel();
        this.BindingContext = viewModel;
        IsScanMode = true;
    }

    public PickingView(string barcode)
    {
        InitializeComponent();
        viewModel = new PickingViewModel(barcode);
        this.BindingContext = viewModel;
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (IsScanMode)
        {
            Methods.AskForRequiredPermissionAsync();

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (AppValues.LeftScanIcon)
        {
            header.FlowDirection = FlowDirection.RightToLeft;
        }
    }

    private void CameraOnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        if (e.BarcodeResults.Length <= 0)
            return;
        gridData.IsVisible = true;
        mainContianer.SetRow(gridData, 1);
        nativeBarcode.PauseScanning = true;
        nativeBarcode.CameraEnabled = false;
        _ = viewModel.FetchStockDetails(e.BarcodeResults[0].DisplayValue);
        // Dispatcher.DispatchAsync(() => { });
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

    private void TapToScan(object sender, TappedEventArgs e)
    {
        //nativeBarcode.PauseScanning = false;
        //nativeBarcode.TapToFocusEnabled = true;
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
        viewModel.IsVisibleEditQuantityAndExpiryPopup = false;
        entry.HideSoftInputAsync(CancellationToken.None);
    }

    void OpenPopup()
    {
        popup.IsVisible = true;
        backgroundTransparence.IsVisible = true;
        nativeBarcode.PauseScanning = true;
    }



    private void Popup_HideSoftInput_Tapped(object sender, TappedEventArgs e)
    {
        entry.HideSoftInputAsync(CancellationToken.None);
    }
}