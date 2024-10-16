using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class AllStockView : ContentPage
{
    public AllStockView()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        var vm = BindingContext as AllStockViewModel;
        if (vm!= null &&  vm.BottomSheet)
        {
            vm.CloseOpenBottomSheet();
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
            await Alerts.DisplaySnackbar("1_CheckBox");
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            (BindingContext as AllStockViewModel)!.ActivityIndicatorRunning = true;
            if (e.Parameter != null)
            {
                var item = (ProductDto)e.Parameter;
                //AppValues.NavigationProductCode = item.ProductCode;
                await Navigation.PushAsync(new PickingView(item.ProductCode), true);
            }
            (BindingContext as AllStockViewModel)!.ActivityIndicatorRunning = false;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar("TapGestureRecognizer_Tapped" + ex.Message);
        }
    }
}