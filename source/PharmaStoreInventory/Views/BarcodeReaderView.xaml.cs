using BarcodeScanning;

namespace PharmaStoreInventory.Views;

public partial class BarcodeReaderView : ContentPage
{
    public BarcodeReaderView()
    {

        this.FlowDirection = FlowDirection.RightToLeft;
        InitializeComponent();

        Methods.AskForRequiredPermissionAsync();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        nativeBarcode.CameraEnabled = true;
        nativeBarcode.PauseScanning = false;
    }
    //private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    //{


    //    var first = e.Results.FirstOrDefault();
    //    if (first is null)
    //        return;


    //    Dispatcher.DispatchAsync(async () =>
    //    {
    //        await DisplayAlert("NavigationProductCode Detected", first.Value, "OK");

    //    });
    //}


    private void CameraView_OnDetectionFinished_1(object sender, OnDetectionFinishedEventArg e)
    {
        var first = e.BarcodeResults.FirstOrDefault();
        if (first is null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {
            //await DisplayAlert("NavigationProductCode Detected", first.DisplayValue, "OK");
            //await Navigation.PushAsync(new StockDetailsView(first.DisplayValue) ,false);
            await Navigation.PushAsync(new StockDetailsView(), false);

        });
        nativeBarcode.PauseScanning = true;

    }

    private void Button_Clicked1(object sender, EventArgs e)
    {
        nativeBarcode.CameraEnabled = false;
        nativeBarcode.IsVisible = false;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
        //cameraViewContianer.IsVisible = false;
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        //cameraViewContianer.RemoveById(nativeBarcode);
        await Navigation.PopAsync();

    }
}