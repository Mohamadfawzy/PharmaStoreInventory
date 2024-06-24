using BarcodeScanning;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace PharmaStoreInventory.Views;

public partial class BarcodeReaderView : ContentPage
{
    public BarcodeReaderView()
    {
        InitializeComponent();
        //barcodeReader.Options = new ZXing.Net.Maui.BarcodeReaderOptions
        //{
        //    //Formats = ZXing.Net.Maui.BarcodeFormat.DataMatrix,
        //    Formats = BarcodeFormats.OneDimensional,
        //    AutoRotate = true,
        //    Multiple = true
        //};

        Methods.AskForRequiredPermissionAsync();


        nativeBarcode.CameraEnabled = true;
        
    }

    private void barcodeReader_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        //barcodeReader.IsTorchOn = !barcodeReader.IsTorchOn;

        var first = e.Results.FirstOrDefault();
        if (first is null)
            return;


        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("Barcode Detected", first.Value, "OK");

            //});
            //barcodeReader.IsDetecting = false;
            //barcodeReader.IsVisible = false;//.AutoFocus();

        });
    }


    private void CameraView_OnDetectionFinished_1(object sender, OnDetectionFinishedEventArg e)
    {
        var first = e.BarcodeResults.FirstOrDefault();
        if (first is null)
            return;

        Dispatcher.DispatchAsync(async () =>
        {
            await DisplayAlert("Barcode Detected", first.DisplayValue, "OK");

        });
        nativeBarcode.PauseScanning = true;
        
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // barcodeReader.IsTorchOn = !barcodeReader.IsTorchOn;
        nativeBarcode.TorchOn = !nativeBarcode.TorchOn;
    }
    
    private void Button_Clicked1(object sender, EventArgs e)
    {
        nativeBarcode.CameraEnabled = false;
        nativeBarcode.IsVisible = false;
    }
}