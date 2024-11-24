using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class LoginView : ContentPage
{
    private readonly JsonFileHanler jsonFileHandler;

    #region OnStart
    public LoginView()
    {
        InitializeComponent();
        jsonFileHandler = new JsonFileHanler(AppValues.UserFileName);
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        AppPreferences.SetDeviceID();
        if (Validations.Validator.IsNetworkAccess())
        {
            NetworkNotAccessAlert();
        }
        SetValues();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Helpers.AppPreferences.SetDeviceID();
        password.InputText = string.Empty;
    }
    #endregion

    #region OnClicked
    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
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
            if (Validations.Validator.IsNetworkAccess())
            {
                NetworkNotAccessAlert();
                LoginEnded();
                return;
            }

            // if eny input is invalid
            if (check == InputType.Empty)
            {
                LoginEnded();
                return;
            }

            var userModel = new UserLoginRequestDto()
            {
                EmailOrPhone = email.InputText,
                Password = password.InputText,
                IsNewDevice = newDeviceCheckBox.IsChecked,
                DviceId = AppPreferences.GetDeviceID()
            };

            if (check == InputType.Phone)
            {
                res = await ApiServices.UserLoginByPhoneAsync(userModel);
            }
            else if (check == InputType.Email)
            {
                res = await ApiServices.UserLoginByEmailAsync(userModel);
            }

            // server in not running
            if (res == null)
            {
                notification.Display("تحقق من الاتصال بالسيرفر");
                LoginEnded();
                return;
            }

            if (res.IsSuccess)
            {
                // Save user data in json file
                if (res.Data == null)
                {
                    LoginEnded();
                    notification.Display("حدث خطأ غير معروف");
                    return;
                }
                AppPreferences.HostUserId = res.Data.Id;
                AppPreferences.IsLoggedIn = true;
                AppPreferences.IsUserActivated = true;
                AppPreferences.UserEmail = userModel.EmailOrPhone;
                AppPreferences.UserFullName = res.Data.FullName;

                _ = jsonFileHandler.WriteToFile(res.Data);
                _ = Alerts.DisplayToast("Welcome: " + res.Data.FullName);

                // Navigation
                await Navigation.PushAsync(new BranchesView(false));

                Navigation.RemovePage(this);
            }

            // if Failure
            else
            {
                if (res.Data != null)
                    AppPreferences.UserFullName = res.Data.FullName;

                if (res.ErrorCode == ErrorCode.UserNotActive)
                {
                    AppPreferences.IsUserActivated = false;
                    AppPreferences.UserEmail = userModel.EmailOrPhone;
                    await Navigation.PushAsync(new WaitingApprovalView());
                    Navigation.RemovePage(this);
                }
                else if (res.ErrorCode == ErrorCode.AccessLimitation)
                {
                    notification.Display("تقيد الوصول", "يرجي الخروج من الهاتف القديم،او يمكنك طلب تسجيل جهاز جديد");
                    newDviceStack.IsVisible = true;
                    AppPreferences.UserEmail = userModel.EmailOrPhone;
                }
                else if (res.ErrorCode == ErrorCode.EmailNotExist)
                {
                    notification.Display("هذا الايميل غير موجود");
                }
                else if (res.ErrorCode == ErrorCode.PhoneNumberNotExist)
                {
                    notification.Display("رقم الهاتف غير موجود");
                }
                else if (res.ErrorCode == ErrorCode.PasswordIsIncorrect)
                {
                    notification.Display("الرقم السري غير صحيح");
                }
                else if (res.ErrorCode == ErrorCode.ExceptionError)
                {
                    notification.Display("حدث خطأ ما  من جهة السيرفر");
                }
            }

            //end try
            LoginEnded();
        }
        catch
        {
            LoginEnded();
        }
    }

    private async void GoToRegisterViewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
        Navigation.RemovePage(this);
    }

    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
        if (Validations.Validator.IsNetworkAccess())
        {
            NetworkNotAccessAlert();
        }
        refreshView.IsRefreshing = false;
    }

    private void NewDeviceStack_Tapped(object sender, TappedEventArgs e)
    {
        newDeviceCheckBox.IsChecked = !newDeviceCheckBox.IsChecked;
    }

    private async void GoToResetPasswordView_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordView());
    }

    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        if (AppValues.IsDevelopment)
        {
            email.SetInputText("mofawzyhelal@gmail.com");
            password.SetInputText("123");
        }
    }
    #endregion

    #region On Call API
    #endregion

    #region On process
    void SetValues()
    {
        if (!string.IsNullOrEmpty(AppPreferences.UserEmail))
        {
            email.SetInputText(AppPreferences.UserEmail);
        }

    }
    void LoginEnded()
    {
        loginButton.IsEnabled = true;
        activityIndicator.IsRunning = false;
        refreshView.IsRefreshing = false;
    }
    private InputType CheckInputs()
    {
        InputType inputType = InputType.Empty;

        if (string.IsNullOrEmpty(email.InputText))
        {
            email.IsError = true;
        }
        else
        {
            if (PharmaStoreInventory.Validations.Validator.IsValidTelephone(email.InputText))
            {
                inputType = InputType.Phone;
            }
            else if (PharmaStoreInventory.Validations.Validator.IsValidEmail(email.InputText))
            {
                inputType = InputType.Email;
            }
            else
            {
                email.IsError = true;
                email.ErrorMessage = "تحقق من المدخلات";
            }
        }

        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            inputType = InputType.Empty;
        }

        return inputType;
    }// end IsAnyInvalidInput
    private void NetworkNotAccessAlert()
    {
        notification.Display(new Models.ErrorMessage("No NetworkAccess", "please check your WiFi"));
    }
    #endregion
}