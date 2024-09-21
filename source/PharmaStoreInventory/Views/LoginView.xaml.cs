using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class LoginView : ContentPage
{
    private readonly JsonFileHanler jsonFileHanler;
    private readonly XmlFileHandler xFileHanler;
    public LoginView()
    {
        InitializeComponent();
        jsonFileHanler = new JsonFileHanler(AppValues.UserFileName);
        xFileHanler = new(AppValues.XBranchsFileName);
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        AppPreferences.SetDeviceID();
        if (Validations.Validator.IsNetworkAccess())
        {
            NetworkNotAccessAlert();
        }
    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Helpers.AppPreferences.SetDeviceID();
        password.InputText = string.Empty;
    }

    private async void LoginClicked(object sender, EventArgs e)
    {
        try
        {
            loginButton.IsEnabled = false;
            activityIndicator.IsRunning = true;

            Result<UserLoginResponseDto>? res = null;
            InputType check = CheckInputs();

            // if No Enternet
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
                notification.ShowMessage("تحقق من الاتصال بالسيرفر");
                LoginEnded();
                return;
            }

            if (res.IsSuccess)
            {
                // Save user data in json file
                if (res.Data == null)
                {
                    LoginEnded();
                    notification.ShowMessage("حدث خطأ غير معروف");
                    return;
                }
                AppPreferences.HostUserId = res.Data.Id;
                AppPreferences.IsLoggedIn = true;
                AppPreferences.IsUserActivated = true;

                _ = jsonFileHanler.WriteToFile(res.Data);
                _ = Alerts.DisplayToast("Welcom: " + res.Data.FullName);

                // Navigation
                await Navigation.PushAsync(new BranchesView());

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
                    await Navigation.PushAsync(new WaitingApprovalView());
                    Navigation.RemovePage(this);
                }
                else if (res.ErrorCode == ErrorCode.AccessLimitation)
                {
                    notification.ShowMessage("تقيد الوصول", "يرجي الخروج من الهاتف القديم،او يمكنك طلب تسجيل جهاز جديد");
                    newDviceStack.IsVisible = true;
                }
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

            //end try
            LoginEnded();
        }
        catch
        {
            LoginEnded();
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
            if (DataAccess.DomainModel.Validator.IsValidTelephone(email.InputText))
            {
                inputType = InputType.Phone;
            }
            else if (DataAccess.DomainModel.Validator.IsValidEmail(email.InputText))
            {
                inputType = InputType.Email;
            }
            //else if (email.InputText == "admin")
            //{
            //    inputType = InputType.Admin;
            //}
        }

        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            inputType = InputType.Empty;
        }

        return inputType;
    }// end CheckInputs

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

    private void PhoneDialerTapped(object sender, TappedEventArgs e)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open("01200007275");
    }

    private void NetworkNotAccessAlert()
    {
        notification.ShowMessage(new Models.ErrorMessage("No NetworkAccess", "please check your WiFi"));
    }

    private void NewDviceStack_Tapped(object sender, TappedEventArgs e)
    {
        newDeviceCheckBox.IsChecked = !newDeviceCheckBox.IsChecked;
    }
}