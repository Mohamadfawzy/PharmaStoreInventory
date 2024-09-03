namespace PharmaStoreInventory.Views;

public partial class DashboardView : ContentPage
{
	public DashboardView()
	{
		InitializeComponent();
	}

    private void HamburgerTapped(object sender, TappedEventArgs e)
    {
		Navigation.PushAsync(new UserView());
    }

    private async void GoToScanPage(object sender, TappedEventArgs e)
    {
        //Navigation.PushAsync(new Views.BarcodeReaderView());
        //await Navigation.PushAsync(new Views.AllStockView());
        await Navigation.PushAsync(new Views.PickingView());
    }

    private void OpenPopupTapped(object sender, TappedEventArgs e)
    {
        OpenPopup();
    }

    private void ClosePopupTapped(object sender, TappedEventArgs e)
    {
        ClosePopup();
    }

    void ClosePopup()
    {
        backgroundTransparence.IsVisible = false;
        popup.IsVisible = false;
    }

    void OpenPopup()
    {
        backgroundTransparence.IsVisible = true;
        popup.IsVisible = true;
    }

    private async void GoToAllProductsPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AllStockView());
    }
}