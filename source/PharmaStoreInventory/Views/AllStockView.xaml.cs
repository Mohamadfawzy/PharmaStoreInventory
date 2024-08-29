using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class AllStockView : ContentPage
{
    public AllStockView()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        if (filterIsOpen)
        {
            _ = CloseFilterFrameAninmation();
            return true;
        }
        return base.OnBackButtonPressed();
    }

    //private void AllStockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (allStockList.SelectedItem != null)
    //    {
    //        var item = (ProductDto)allStockList.SelectedItem;
    //        AppValues.NavigationProductCode = item.ProductCode;
    //    }
    //    Navigation.PushAsync(new PickingView());
    //}
    bool filterIsOpen = false;
    private async void OpenFiltrFrame(object sender, TappedEventArgs e)
    {
        try
        {
            filterIsOpen = true;
            borderFilter.TranslationY = 800;
            borderFilter.IsVisible = true;
            await borderFilter.TranslateTo(0, 0, length: 500, easing: Easing.CubicInOut);
        }
        catch (Exception)
        {

        }
    }

    private async void CloseFiltrFrame(object sender, EventArgs e)
    {
        await CloseFilterFrameAninmation();
    }

    private async Task CloseFilterFrameAninmation()
    {
        try
        {
            filterIsOpen = false;
            await borderFilter.TranslateTo(0, 800, length: 500, easing: Easing.CubicInOut);
            borderFilter.IsVisible = false;
        }
        catch (Exception)
        {
        }
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        if (checkBox?.ClassId == "CheckBox_1")
        {
            await Helpers.Alerts.DisplaySnackbar("1_CheckBox");
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter != null)
        {
            var item = (ProductDto)e.Parameter;
            //AppValues.NavigationProductCode = item.ProductCode;
            await Navigation.PushAsync(new PickingView(item.ProductCode), true);
        }
    }
}