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

    private void GoToScanPage(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new Views.BarcodeReaderView());
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


}