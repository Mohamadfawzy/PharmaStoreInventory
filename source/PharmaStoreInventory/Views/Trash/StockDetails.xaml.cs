namespace PharmaStoreInventory.Views.Trash;

public partial class StockDetails : ContentPage
{
	public StockDetails()
	{
		InitializeComponent();
	}
    private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
    {
        DisplayAlert("title", "massage", "cancel");
    }

    private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        DisplayAlert("title", "massage", "cancel");

    }

}