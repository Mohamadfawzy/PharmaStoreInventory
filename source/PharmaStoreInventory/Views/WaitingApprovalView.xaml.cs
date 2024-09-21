using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

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

    private void PhoneDialerTapped(object sender, TappedEventArgs e)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open("01200007275");
    }

    //private void RefreshTapped(object sender, TappedEventArgs e)
    //{
    //    RefreshStatus();
    //}
    
    private async void LogoutTapped(object sender, TappedEventArgs e)
    {
        //if (Application.Current != null)
        //    Application.Current.MainPage = new NavigationPage(new LoginView());
        //AppPreferences.IsLoggedIn = false;
        //AppPreferences.HostUserId = 0;
        await Navigation.PushAsync(new LoginView());
        Navigation.RemovePage(this);
    }

    //private async void RefreshStatus()
    //{
    //    if (AppPreferences.HostUserId == 0)
    //        return;

    //    var res = await ApiServices.IsUserActiveAsync(AppPreferences.HostUserId);
    //    if (res != null && res.IsSuccess)
    //    {
    //        await Navigation.PushAsync(new CreateBranchView());
    //        AppPreferences.IsUserActivated = true;
    //    }
    //}
}