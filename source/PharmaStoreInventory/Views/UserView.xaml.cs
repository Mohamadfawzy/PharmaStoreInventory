using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    private JsonFileHanler jsonFileHanler;
    public UserView()
    {
        InitializeComponent();
        jsonFileHanler = new JsonFileHanler(AppValues.UserFileName);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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
        if (e.Parameter == null)
            return;
        string parameter = (string)e.Parameter;

        if (parameter == "Branches")
        {
            await Navigation.PushAsync(new BranchesView());
        }
        else if (parameter == "resetPassword")
        {
            OpenPopup();
        }
        else if (parameter == "Logout")
        {
           Logout();
        }
        else
        {

        }

    }

    private void Logout()
    {
        if (Application.Current != null)
            Application.Current.MainPage = new NavigationPage(new LoginView());
        AppPreferences.IsLoggedIn = false;
        AppPreferences.HostUserId = 0;
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
}