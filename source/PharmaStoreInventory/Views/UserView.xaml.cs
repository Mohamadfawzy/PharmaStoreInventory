using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    private string userEmail = string.Empty;
    private JsonFileHanler jsonFileHandler;
    UserLoginResponseDto? CurrentUserData;

    #region OnStart
    public UserView()
    {
        InitializeComponent();
        jsonFileHandler = new JsonFileHanler(AppValues.UserFileName);
    }

    private async void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        jsonFileHandler = new JsonFileHanler(AppValues.UserFileName);

        CurrentUserData = await jsonFileHandler.SingleObject<UserLoginResponseDto>();
        if (CurrentUserData != null)
        {
            userEmail = CurrentUserData.Email ?? "";
            nameEntry.Text = CurrentUserData.FullName;
            pharmaName.Text = CurrentUserData.PharmcyName;
            phone.Text = CurrentUserData.PhoneNumber;
            email.Text = CurrentUserData.Email;
        }
        nameEntry.Focus();
        saveButton.IsEnabled = false;
    }
    #endregion

    #region OnClicked
    private void ClosePopupClicked(object sender, EventArgs e)
    {
        ClosePopup();
    }

    private void Popup_HideSoftInput_Tapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private void ClosePopupTapped(object sender, TappedEventArgs e)
    {
        ClosePopup();
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

    private void TouchBehavior_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        OpenPopup();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
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
                FullName = userDataDto.FullName,
                Email = userEmail,
            };
            await jsonFileHandler.WriteToFile(model);
            saveButton.IsEnabled = false;
            notification.Display("تم حفظ التغيرات");
            await nameEntry.HideSoftInputAsync(CancellationToken.None);

        }
        else if (res.ErrorCode == ErrorCode.PhoneNumberAlreadyExists)
        {
            notification.Display("رقم الهاتف مستخدم");
        }
    }

    private async void AnyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (await AnyEntryChanges())
            saveButton.IsEnabled = true;
        else
            saveButton.IsEnabled = false;
    }
    #endregion

    #region On Call API
    #endregion

    #region On process

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

    private Task<bool> AnyEntryChanges()
    {
        var flag = false;
        if (CurrentUserData == null)
            return Task.FromResult(flag);

        if (CurrentUserData.FullName != nameEntry.Text)
        {
            flag = true;
        }

        if (CurrentUserData.PharmcyName != pharmaName.Text)
        {
            flag = true;
        }

        if (CurrentUserData.PhoneNumber != phone.Text)
        {
            flag = true;
        }
        return Task.FromResult(flag);
    }

    #endregion
}