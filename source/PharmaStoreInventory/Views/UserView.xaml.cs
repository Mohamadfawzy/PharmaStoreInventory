using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    private JsonFileHanler jsonFileHandler;
    private string userEmail= string.Empty;
    public UserView()
    {
        InitializeComponent();
        jsonFileHandler = new JsonFileHanler(AppValues.UserFileName);
    }

    private async void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        jsonFileHandler = new JsonFileHanler(AppValues.UserFileName);

        var user = await jsonFileHandler.SingleObject<UserLoginResponseDto>();
        if (user != null)
        {
            userEmail = user.Email?? "";
            nameEntry.Text = user.FullName;
            pharmaName.Text = user.PharmcyName;
            phone.Text = user.PhoneNumber;
            email.Text = user.Email;
        }
        nameEntry.Focus();
    }

    //private void AllowNameEditingTapped(object sender, TappedEventArgs e)
    //{
    //    saveButton.IsVisible = true;
    //    //editIcon.IsVisible = false;
    //    //nameStack.BackgroundColor = Color.FromArgb("#f0f7ff");

    //    nameEntry.IsReadOnly = false;
    //    // email.IsReadOnly = false;
    //    pharmaName.IsReadOnly = false;
    //    // phone.IsReadOnly = false;

    //    nameEntry.Focus();

    //}

    //private void saveButton_Clicked(object sender, EventArgs e)
    //{
    //    saveButton.IsVisible = false;
    //    //editIcon.IsVisible = true;
    //    //nameStack.BackgroundColor = Colors.Transparent;
    //    nameEntry.Unfocus();
    //    nameEntry.IsReadOnly = true;
    //    pharmaName.IsReadOnly = true;
    //    //email.IsReadOnly = true;
    //    //phone.IsReadOnly = true;
    //}

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
        errorLabel.IsVisible = false;
        var model = new ChangePasswordRequest()
        {
            UserId = AppPreferences.HostUserId,
            CurrentPassword = oldPassword.InputText,
            NewPassword = newPassword.InputText,
            ConfirmNewPassword = confirmPassword.InputText,
        };

        var res = await ApiServices.UserChangePasswordAsync(model);
        if (res != null && res.IsSuccess)
        {
            await Alerts.DisplayToast("تم التحديث");
            ClosePopup();
        }
        else
        {
            errorLabel.IsVisible = true;
            errorLabel.Text = "لم يتم تحديث الرقم السري";
        }
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


    private void TouchBehavior_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        OpenPopup();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var userDataDto = new UserEditDataDto()
        {
            FullName = nameEntry.Text,
            PharmacyName = pharmaName.Text,
            PhoneNumber = phone.Text,
            Id = AppPreferences.HostUserId,
        };
        var res = await ApiServices.EditUserDataAsync(userDataDto);
        if (res == null)
        {
            return;
        }

        if (res.IsSuccess)
        {
            var model = new UserLoginResponseDto()
            {
                Id = userDataDto.Id,
                PharmcyName = userDataDto.PharmacyName,
                PhoneNumber = userDataDto.PhoneNumber,
                FullName = userEmail,
            };
            await jsonFileHandler.WriteToFile(model);
            notification.ShowMessage("تم حفظ التغيرات");

        }
        else if (res.ErrorCode == ErrorCode.PhoneNumberAlreadyExists)
        {
            notification.ShowMessage("رقم الهاتف مستخدم");
        }



    }

    private void AnyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}