using PharmaStoreInventory.Models;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class StockDetailsView : ContentPage
{

    public StockDetailsView(string code= "6221068000977")
    {
        InitializeComponent();
        this.BindingContext = new StockDetailsViewModel(code);
    }

    private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
    {
        DisplayAlert("title", "massage", "cancel");
    }

    private void OnDoneSwipeItemInvoked(object sender, EventArgs e)
    {

        //DisplayAlert("title", "massage", "cancel");
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var item = btn.CommandParameter as StockModel;
        
    }

    private async void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {

        if (e.SwipeDirection == SwipeDirection.Right)
        {
            var swip = sender as SwipeView;
            //var first = swip?.LeftItems.First() as SwipeItemView;
            var checkIcon = swip?.FindByName("checkIcon") as Label;
            if (checkIcon != null)
            {
                await checkIcon.TranslateTo(0, 10);
                await checkIcon.ScaleTo(1.2, 400);
                await checkIcon.ScaleTo(0.8, 400);
                await checkIcon.ScaleTo(1, 400);
                await checkIcon.TranslateTo(0, 0);
            }
        }
    }

    void GetStockDetails(string code)
    {
        var list = Services.MockData.GetStockByBarcode(code);
        collection.ItemsSource = list;
        nameAr.Text = list.FirstOrDefault()?.ItemNameArabic;
        nameEn.Text = list.FirstOrDefault()?.ItemNameEnglish;
        barcode.Text = code;
    }

}