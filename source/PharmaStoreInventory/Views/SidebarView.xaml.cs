using DataAccess.DomainModel;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class SidebarView : ContentPage
{
	public SidebarView()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        userName.Text = AppPreferences.UserFullName;
        userEmail.Text = AppPreferences.UserEmail;
    }

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void Logout()
    {
        File.Delete(AppValues.XBranchesFileName);
        File.Delete(AppValues.UserFileName);
        File.Delete(AppValues.BranchesFileName);


        AppPreferences.LocalDbUserId = 0;
        AppPreferences.StoreId = 1;
        AppPreferences.HasBranchRegistered = false;
        AppPreferences.LeftScanIcon = false;


        if (Application.Current != null)
            Application.Current.MainPage = new NavigationPage(new LoginView());
        AppPreferences.IsLoggedIn = false;
        AppPreferences.HostUserId = 0;

        // delete all be low
        AppPreferences.StoreId = 0;
    }

    private async void Reset()
    {
        File.Delete(AppValues.XBranchesFileName);
        File.Delete(AppValues.UserFileName);
        File.Delete(AppValues.BranchesFileName);

        AppPreferences.HostUserId = 0;
        AppPreferences.LocalDbUserId = 0;
        AppPreferences.StoreId = 1;
        AppPreferences.IsLoggedIn = false;
        AppPreferences.IsFirstTime = true;
        AppPreferences.IsUserActivated = false;
        AppPreferences.HasBranchRegistered = false;
        AppPreferences.LeftScanIcon = false;

        if (Application.Current != null)
            Application.Current.MainPage = new NavigationPage(new OnbordingView());
    }

    private async void TouchBehavior_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        var border = (Border)sender;
        if (e.TouchCommandParameter == null)
            return;
        string parameter = (string)e.TouchCommandParameter;

        if (parameter == "Branches")
        {
            border.BackgroundColor = Color.FromArgb("#400056a9");
            await Navigation.PushAsync(new BranchesView());
        }
        else if (parameter == "AddBranch")
        {
            await Navigation.PushAsync(new CreateBranchView());
        }
        else if (parameter == "Cog")
        {
            await Navigation.PushAsync(new SettingView());
        }
        else if (parameter == "Logout")
        {
            Logout();
        }
        else
        {
            Reset();
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new UserView());
    }
}