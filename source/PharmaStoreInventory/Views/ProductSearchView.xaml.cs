using System.Threading.Tasks;

namespace PharmaStoreInventory.Views;

public partial class ProductSearchView : ContentPage
{
	public ProductSearchView()
	{
		InitializeComponent();
	}

    private async void OnBackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}