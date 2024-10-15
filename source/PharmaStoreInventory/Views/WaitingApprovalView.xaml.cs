using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class WaitingApprovalView : ContentPage
{
    public WaitingApprovalView()
    {
        InitializeComponent();
    }
    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        userFullName.Text =  AppPreferences.UserFullName;
    }

    private async void LogoutTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
        Navigation.RemovePage(this);
    }
}