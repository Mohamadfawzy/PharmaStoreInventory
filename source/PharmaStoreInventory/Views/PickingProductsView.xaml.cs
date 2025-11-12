using BarcodeScanning;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using PharmaStoreInventory.ViewModels;
using System.Threading.Tasks;

namespace PharmaStoreInventory.Views;

public partial class PickingProductsView : ContentPage
{
    PickingProductsViewModel viewModel;
    public PickingProductsView()
    {
        Methods.AskForRequiredPermissionAsync();
        InitializeComponent();
        //WeakReferenceMessenger.Default.Register<PickingViewNotification>(this);

        viewModel = new PickingProductsViewModel();
        this.BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            nativeBarcode.PauseScanning = false;
            nativeBarcode.CameraEnabled = true;
            btnFlashLight.IsVisible = true;
            btnBarcodeScan.IsVisible = false;
            viewModel.ActivityIndicatorRunning = false;
        });
    }

    private async void CameraOnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
    {
        await Dispatcher.DispatchAsync(() =>
        {
            if (e.BarcodeResults.Length <= 0)
                return;
            viewModel.ActivityIndicatorRunning = true;
            ToggleCameraVisibility();
            viewModel.FetchStockDetails(e.BarcodeResults[0].DisplayValue);
        });
    }

    private void ToggleCameraVisibility()
    {

        btnFlashLight.IsVisible = !btnFlashLight.IsVisible;
        btnBarcodeScan.IsVisible = !btnBarcodeScan.IsVisible;
        //nativeBarcode.IsVisible = !nativeBarcode.IsVisible;
        nativeBarcode.PauseScanning = !nativeBarcode.PauseScanning;
        nativeBarcode.CameraEnabled = !nativeBarcode.CameraEnabled;
        //mainContianer.SetRow(gridData, IsScanMode ? 2 : 1);
    }

    private void ToggleFlashLight_Clicked(object sender, EventArgs e)
    {
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductSearchView());

    }

    private void btnBarcodeScan_Clicked(object sender, EventArgs e)
    {
        ToggleCameraVisibility();
        viewModel.NotFoundVisible = false;
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}