using PharmaStoreInventory.Extensions;

namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage
{
	public CreateBranchView()
	{
		InitializeComponent();
	}
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }
}