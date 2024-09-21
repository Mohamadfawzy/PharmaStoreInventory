using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    //private JsonFileHanler jsonFileHanler;
    public UserView()
    {
        InitializeComponent();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }
    private async void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var jsonFileHanler = new JsonFileHanler(AppValues.UserFileName);

        var user = await jsonFileHanler.SingleObject<UserLoginResponseDto>();
        if (user != null)
        {
            nameEntry.Text = user.FullName;
            pharmaName.Text = user.PharmcyName;
            phone.Text = user.PhoneNumber;
            email.Text = user.Email;
        }
    }

    private void AllowNameEditingTapped(object sender, TappedEventArgs e)
    {
        saveButton.IsVisible = true;
        editIcon.IsVisible = false;
        nameStack.BackgroundColor = Color.FromArgb("#f0f7ff");

        nameEntry.IsReadOnly = false;
        // email.IsReadOnly = false;
        pharmaName.IsReadOnly = false;
        // phone.IsReadOnly = false;

        nameEntry.Focus();

    }

    private void saveButton_Clicked(object sender, EventArgs e)
    {
        saveButton.IsVisible = false;
        editIcon.IsVisible = true;
        nameStack.BackgroundColor = Colors.Transparent;
        nameEntry.Unfocus();
        nameEntry.IsReadOnly = true;
        pharmaName.IsReadOnly = true;
        //email.IsReadOnly = true;
        //phone.IsReadOnly = true;
    }

    private void ClosePopupClicked(object sender, EventArgs e)
    {
        ClosePopup();
    }

    private void Popup_HideSoftInput_Tapped(object sender, TappedEventArgs e)
    {
        //confirmPassword.HideKeyBoard();
        inputsContainer.ClearFocusFromAllInputs();

    }

    private void ClosePopupTapped(object sender, TappedEventArgs e)
    {
        ClosePopup();
    }

    void ClosePopup()
    {
        confirmPassword.HideKeyBoard();
        backgroundTransparence.IsVisible = false;
        popup.IsVisible = false;
        oldPassword.ClearText();
        newPassword.ClearText();
        confirmPassword.ClearText();
    }

    private async void MenuItemTapped(object sender, TappedEventArgs e)
    {
        var border = (Border)sender;
        if (e.Parameter == null)
            return;
        string parameter = (string)e.Parameter;

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
        else if (parameter == "ResetPassword")
        {
            OpenPopup();
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


    void OpenPopup()
    {
        backgroundTransparence.IsVisible = true;
        popup.IsVisible = true;
    }


    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SaveChangePasswordClicked(object sender, EventArgs e)
    {
        var model = new ChangePasswordRequest()
        {
            UserId = Helpers.AppPreferences.HostUserId,
            CurrentPassword = oldPassword.InputText,
            NewPassword = newPassword.InputText,
            ConfirmNewPassword = confirmPassword.InputText,
        };

        var res = await ApiServices.UserChangePasswordAsync(model);
        if (res != null && res.IsSuccess)
        {
            await Helpers.Alerts.DisplaySnackbar(res.Message);
        }
    }

    private void Logout()
    {
        File.Delete(AppValues.XBranchsFileName);
        File.Delete(AppValues.UserFileName);
        File.Delete(AppValues.BranchsFileName);


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
        File.Delete(AppValues.XBranchsFileName);
        File.Delete(AppValues.UserFileName);
        File.Delete(AppValues.BranchsFileName);

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
        else if (parameter == "ResetPassword")
        {
            OpenPopup();
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
}