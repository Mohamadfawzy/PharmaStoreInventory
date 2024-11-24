using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Helper;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.Views.Templates;
using System.Diagnostics;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage, IRecipient<RegisterViewNotification>
{
    private string verificationCodeSent = string.Empty;
    bool isEmailVerified = false;
    private UserRegisterDto uRegister = new();
    private readonly MailingService mailingService;

    #region OnStart
    public RegisterView()
    {
        InitializeComponent();
        mailingService = new();
        WeakReferenceMessenger.Default.Register<RegisterViewNotification>(this);
    }

    public void Receive(RegisterViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.Display(message.Value);
        });
    }
    #endregion

    #region OnClicked
    private void ClearFocusFromAllInputs_Tapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        activityIndicator.IsRunning = true;
        try
        {
            if (IsAnyInvalidInput())
            {
                return;
            }

            uRegister = new()
            {
                FullName = fullName.InputText,
                ConfirmNewPassword = confirmPassword.InputText,
                Password = password.InputText,
                Email = email.InputText,
                PharmcyName = pharmacyName.InputText,
                PhoneNumber = telephone.InputText,
                DeviceID = AppPreferences.GetDeviceID()
            };

            AppPreferences.UserEmail = uRegister.Email;
            var canRegister = await CanRegisterEmailOrPhone(uRegister.Email, uRegister.PhoneNumber);
            if (canRegister == false)
            {
                return;
            }
            ShowVerificationViewTemplate();
            SaveAndSendVerificationCode();
        }
        catch (Exception ex)
        {
            notification.Display("exception", ex.Message);
        }
        finally
        {
            activityIndicator.IsRunning = false;
        }
    }

    private async void VerificationViewTemplate_SubmitClicked(object sender, EventArgs e)
    {
        //Debug.WriteLine($"\n\n\n Guid:{uRegister.EVCID}");
        uRegister.VerificationCode = verificationViewTemplate.GetCode();
        await CreateAccount(uRegister);
    }

    private async void GoToLoginViewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
        Navigation.RemovePage(this);
    }

    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        if (AppValues.IsDevelopment)
        {
            //if(AppPreferences.HasBranchRegistered)
            fullName.SetInputText("محمد هلال");
            confirmPassword.SetInputText("123456");
            password.SetInputText("123456");
            email.SetInputText("mofawzyhelal@gmail.com");
            pharmacyName.SetInputText("صيدلية 1");
            telephone.SetInputText("01093052427");
        }
    }
    #endregion

    #region On Call API
    private async void SaveAndSendVerificationCode()
    {
        try
        {
            //1 save code in database
            EmailRequestModel emailModel = new()
            {
                EVCID = Guid.NewGuid(),
                UserFullName = fullName.InputText,
                Recipient = email.InputText,
            };
            //uRegister.VerificationCode = emailModel.VerificationCode;
            uRegister.EVCID = emailModel.EVCID;
            var saveCodeResponse = await ApiServices.PostAndSendEmail(emailModel);

            //2 send email
            if (saveCodeResponse != null && saveCodeResponse.IsSuccess)
            {
                notification.Display("تم ارسال كود التحقق");
            }
            else
            {
                notification.Display("حدثت مشكلة ولم يتم ارسال الكود");
            }
        }
        catch (Exception ex)
        {
            notification.Display(ex.Message);
        }
        finally { activityIndicator.IsRunning = false; }
    }

    private async Task<bool> CanRegisterEmailOrPhone(string uEmail, string uPhone)
    {
        var res = await ApiServices.CanRegisterEmailOrPhoneAsync(uEmail, uPhone);

        if (res == null)
        {
            notification.Display("تحقق من الاتصال الانترنت");
            return false;
        }

        if (res.IsSuccess)
        {
            return true;
        }

        if (res.Errors != null && res.Errors.Count > 0)
        {
            foreach (var err in res.Errors)
            {
                if (err == ErrorCode.EmailAlreadyExists)
                {
                    email.IsError = true;
                    email.ErrorMessage = "الايميل موجود بالفعل";
                }
                else if (err == ErrorCode.PhoneNumberAlreadyExists)
                {
                    telephone.IsError = true;
                    telephone.ErrorMessage = "رقم الهاتف موجود";
                }
                else if (err == ErrorCode.NullValue)
                {
                    notification.Display("تحقق من إدخال كل الحقول");
                }
                else
                {
                    notification.Display("حدث خطأ ما داخل الهوست", res.Message);
                }
            }
        }
        else
        {
            notification.Display(res.Message);
        }
        return false;
    }

    private async Task CreateAccount(UserRegisterDto user)
    {
        try
        {

            var res = await ApiServices.RegisterUserAsync(user);
            if (res == null)
            {
                notification.Display("تحقق من الاتصال بالانترنت");
                return;
            }
            if (res.IsSuccess)
            {
                _ = Alerts.DisplayToast("welcome " + user.FullName);

                await Navigation.PushAsync(new LoginView());
                Navigation.RemovePage(this);
            }
            else
            {
                notification.Display(res.Message);
            }
        }
        catch (Exception ex)
        {
            notification.Display("Exception Error", $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}");
        }
    }
    #endregion

    #region On process
    private void ShowVerificationViewTemplate()
    {
        verificationViewTemplate.IsVisible = true;
        verificationViewTemplate.Init();
        verificationViewTemplate.SetSpanEmail(uRegister.Email);
    }

    /// <returns> true if one or more inputs are invalid.</returns>
    bool IsAnyInvalidInput()
    {
        var enyErrorFound = false;
        foreach (var item in inputsContainer.Children)
        {
            var inputFiled = (AnimatedInput)item;
            if (inputFiled.IsAnyError())
            {
                enyErrorFound = true;
            }
        }

        if (password.InputText != confirmPassword.InputText)
        {
            password.IsError = true;
            confirmPassword.IsError = true;
            enyErrorFound = true;
            password.ErrorMessage = confirmPassword.ErrorMessage = "تحقق من تطابق الرقم السري";
        }
        return enyErrorFound;
    }
    #endregion
}