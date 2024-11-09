using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Languages;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage, IRecipient<RegisterViewNotification>
{
    private string verificationCodeSent = string.Empty;
    bool isEmailVerified = false;
    private UserRegisterDto uRegister = new();
    private readonly MailingService mailingService;

    public RegisterView()
    {
        InitializeComponent();
        mailingService = new();
        WeakReferenceMessenger.Default.Register<RegisterViewNotification>(this);
    }
    private async void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {

    }

    public void Receive(RegisterViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.ShowMessage(message.Value);
        });
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        activityIndicator.IsRunning = true;

        try
        {
            var isError = CheckInputs();
            if (isError)
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
            var anyNotFounded = await AreEmailAndPhoneNonExistentAsync(uRegister.Email, uRegister.PhoneNumber);

            if (anyNotFounded)
            {
                if (isEmailVerified)
                {
                    await CreateAccount(uRegister);
                }
                else
                {
                    verificationViewTemplate.IsVisible = true;
                    verificationViewTemplate.SetSpanEmail(uRegister.Email);
                    var res = await mailingService.SendVerificationCodeAsync(uRegister.Email!, null, uRegister.FullName);
                    if (res != null && res.IsSuccess)
                    {
                        verificationCodeSent = res.Data;
                        notification.ShowMessage("تم ارسال كود تحقق");
                    }
                    else
                    {
                        notification.ShowMessage("حدثت مشكلة ولم يتم ارسال الكود");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            notification.ShowMessage("exception", ex.Message);
        }
        finally { activityIndicator.IsRunning = false; }
    }

    private async Task<bool> AreEmailAndPhoneNonExistentAsync(string uEmail, string uPhone)
    {
        var res = await ApiServices.AreEmailAndPhoneNonExistentAsync(uEmail, uPhone);

        if (res == null)
        {
            notification.ShowMessage("تحقق من الاتصال الانترنت");
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
                    notification.ShowMessage("تحقق من إدخال كل الحقول");
                }
                else
                {
                    notification.ShowMessage("حدث خطأ ما داخل الهوست", res.Message);
                }
            }
        }
        else
        {
            notification.ShowMessage(res.Message);
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
                notification.ShowMessage("تحقق من الاتصال بالانترنت");
                return;
            }
            if (res.IsSuccess)
            {
                _ = Helpers.Alerts.DisplayToast("welcome " + user.FullName);

                await Navigation.PushAsync(new LoginView());
                Navigation.RemovePage(this);
            }
        }
        catch (Exception ex)
        {
            notification.ShowMessage("Exception Error", $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}");
        }
    }

    bool CheckInputs()
    {
        bool isError = false;
        if (string.IsNullOrEmpty(fullName.InputText))
        {
            fullName.IsError = true;
            isError = true;
        }
        if (string.IsNullOrEmpty(pharmacyName.InputText))
        {
            pharmacyName.IsError = true;
            isError = true;
        }
        if (string.IsNullOrEmpty(email.InputText) || email.IsError)
        {
            email.IsError = true;
            isError = true;
        }
        if (string.IsNullOrEmpty(telephone.InputText) || telephone.IsError)
        {
            telephone.IsError = true;
            isError = true;
        }
        if (string.IsNullOrEmpty(password.InputText) ||
            string.IsNullOrEmpty(confirmPassword.InputText) ||
            password.InputText != confirmPassword.InputText)
        {
            password.IsError = true;
            confirmPassword.IsError = true;
            isError = true;
        }
        return isError;
    }

    private void VerificationViewTemplate_SubmitClicked(object sender, EventArgs e)
    {
        if (verificationCodeSent == verificationViewTemplate.GetCode())
        {
            notification.ShowMessage("تم التحقق من الإيميل");
            mainCreationButton.Text = "تأكيد إنشاء الحساب";
            verificationViewTemplate.IsVisible = false;
            email.IsEnabled = false;
            isEmailVerified = true;
        }
        else
        {
            notification.ShowMessage("الكود خاطئ");
        }
    }

    // will Deleted
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

    private async void GoToLoginViewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
        Navigation.RemovePage(this);
    }
}



















