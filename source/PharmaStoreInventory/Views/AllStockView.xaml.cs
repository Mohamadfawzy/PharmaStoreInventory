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
        if (vm != null && vm.BottomSheet)
        {
            vm.CloseOpenBottomSheet();
            return true;
        }
        return base.OnBackButtonPressed();
    }

    private async void GoToPickingViewTapped(object sender, TappedEventArgs e)
    {
        try
        {
            (BindingContext as AllStockViewModel)!.ActivityIndicatorRunning = true;
            if (e.Parameter != null)
            {
                var item = (ProductDto)e.Parameter;
                await Navigation.PushAsync(new PickingView(item.ProductCode), true);
            }
            (BindingContext as AllStockViewModel)!.ActivityIndicatorRunning = false;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar("TapGestureRecognizer_Tapped" + ex.Message);
        }
    }
}