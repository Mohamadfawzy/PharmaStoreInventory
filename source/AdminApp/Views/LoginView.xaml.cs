
using AdminApp.AppData;
using AdminApp.Services.ApiServices;
using DataTransferObjects.UserDTOs;
using Shared.Enums;
using Shared.MAUI.Helpers;
using Shared.Models;

namespace AdminApp.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }
    void SetValues()
    {
        if (!string.IsNullOrEmpty(PreferencesData.UserEmail))
        {
            email.SetInputText(PreferencesData.UserEmail);
        }

    }
    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        //inputsContainer.ClearFocusFromAllInputs();
    }

    private async void LoginClicked(object sender, EventArgs e)
    {
        try
        {
            loginButton.IsEnabled = false;
            activityIndicator.IsRunning = true;

            Result<UserLoginResponseDto>? res = null;
            InputType check = CheckInputs();

            // if No Internet
            if (Networking.IsNetworkAccess())
            {
                notification.ShowMessage("تحقق من الاتصال باالانترنت");
                return;
            }

            // if eny input is invalid
            if (check == InputType.Empty)
            {
                notification.ShowMessage("من فضلك ادخل كل الحقول");
                return;
            }

            var userModel = new LoginDto(email.InputText, password.InputText);
            res = await UserApiService.AdminLoginByEmailAsync(userModel);

            // server in not running
            if (res == null)
            {
                notification.ShowMessage("تحقق من الاتصال بالسيرفر");
                return;
            }

            if (res.IsSuccess)
            {
                // Save user data in json file
                if (res.Data == null)
                {
                    
                    notification.ShowMessage("حدث خطأ غير معروف");
                    return;
                }
                // Navigation
                await Navigation.PushAsync(new UsersView());

                Navigation.RemovePage(this);
            }

            // if Failure
            else
            {
                if (res.Data != null)
                    PreferencesData.UserFullName = res.Data.FullName;

                else if (res.ErrorCode == ErrorCode.EmailNotExist)
                {
                    notification.ShowMessage("هذا الايميل غير موجود");
                }
                else if (res.ErrorCode == ErrorCode.PhoneNumberNotExist)
                {
                    notification.ShowMessage("رقم الهاتف غير موجود");
                }
                else if (res.ErrorCode == ErrorCode.PasswordIsIncorrect)
                {
                    notification.ShowMessage("الرقم السري غير صحيح");
                }
                else if (res.ErrorCode == ErrorCode.ExceptionError)
                {
                    notification.ShowMessage("حدث خطأ ما  من جهة السيرفر");
                }
            }
        }
        catch
        {
            notification.ShowMessage("حدث خطأ غير معروف");
        }
        finally
        {
            LoginEnded();
        }
    }

    void LoginEnded()
    {
        loginButton.IsEnabled = true;
        activityIndicator.IsRunning = false;
    }

    private InputType CheckInputs()
    {
        InputType inputType = InputType.Text;

        if (string.IsNullOrEmpty(email.InputText))
        {
            email.IsError = true;
            inputType = InputType.Empty;
        }

        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            inputType = InputType.Empty;
        }
        
        return inputType;
    }// end CheckInputs

    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        if (StaticData.IsDevelopment)
        {
            email.SetInputText("mofawzyhelal@gmail.com");
            password.SetInputText("123");
        }
    }
}