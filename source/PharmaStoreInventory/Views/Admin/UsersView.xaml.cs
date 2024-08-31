namespace PharmaStoreInventory.Views.Admin;

public partial class UsersView : ContentPage
{
	public UsersView()
	{
		InitializeComponent();
	}

    private async void GoToUserDetailsViewTapped(object sender, TappedEventArgs e)
    {
		await Navigation.PushAsync(new UserDetailsView());
    }
    private void HamburgerTapped(object sender, TappedEventArgs e)
    {
        //Navigation.PushAsync(new UserView());
    }

}