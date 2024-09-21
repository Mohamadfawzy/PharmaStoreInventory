using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage, IRecipient<RegisterViewNotification>
{
    private readonly RegisterViewModel vm;
    private string verificationCodeSended = string.Empty;
    bool isEmailVerified = false;
    private UserRegisterDto uRegister = new();
    private readonly MailingService mailingService;

    public RegisterView()
    {
        InitializeComponent();
        mailingService = new();
        WeakReferenceMessenger.Default.Register<RegisterViewNotification>(this);
        vm = (RegisterViewModel)BindingContext;
    }
    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // when releas remove this
        inputsContainer.PositioningOfPlaceHolder();
        //var verfViewTemplate = new VerificationViewTemplate() { ZIndex = 2 };
        //manStack.SetRowSpan(verfViewTemplate, 3);
        //manStack.Add(verfViewTemplate);
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
            };

            var anyNotFounded = await AreEmailAndPhoneNonExistentAsync(uRegister.Email, uRegister.PhoneNumber);

            if (anyNotFounded)
            {
                if (isEmailVerified)
                {
                    await CreateAcount(uRegister);
                }
                else
                {
                    verificationViewTemplate.IsVisible = true;
                    verificationViewTemplate.SetSpanEmail(uRegister.Email);
                    verificationCodeSended = "1111"; // await mailingService.SendVerificationCodeAsync(uRegister.Email!, null, "mohamed fawzy");
                }
            }
        }
        catch (Exception ex)
        {
            notification.ShowMessage("exception", ex.Message);
        }
        finally { activityIndicator.IsRunning = false; }
    }
    async Task<bool> AreEmailAndPhoneNonExistentAsync(string uEmail, string uPhone)
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
        return false;
    }

    private async Task CreateAcount(UserRegisterDto user)
    {
        try
        {
            var res = await ApiServices.RegisterUserAcync(user);
            if (res == null)
            {
                notification.ShowMessage("تحقق من الاتصال بالانترنت");
                return;
            }
            if (res.IsSuccess)
            {
                _ = Helpers.Alerts.DisplayToast("welcom " + user.FullName);

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
        if (verificationCodeSended == verificationViewTemplate.GetCode())
        {
            notification.ShowMessage("good");
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

    // will deleted
    private async void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        fullName.InputText = "محمد رجب";
        confirmPassword.InputText = "123";
        password.InputText = "123";
        email.InputText = "user1@test.com";
        pharmacyName.InputText = "صيدلية 1";
        telephone.InputText = "01093052421";
        await inputsContainer.PositioningOfPlaceHolder();
    }
    //private async Task<ErrorCode> CheckEmailValidityAndSendEmail(string uEmail, string uPhone)
    //{
    //    try
    //    {
    //        //cheack if email is not exist
    //        var res = await ApiServices.AreEmailAndPhoneNonExistentAsync(uEmail, uPhone);
    //        //_ = Helpers.Alerts.DisplaySnackbar(res.Message);
    //        if (res == null)
    //            return ErrorCode.NullValue;

    //        if (res.IsSuccess)
    //        {
    //            verificationViewTemplate.IsVisible = true;
    //            verificationCodeSended = "1111";// await mailingService.SendVerificationCodeAsync(uRegister.Email!, null, "mohamed fawzy");
    //            return ErrorCode.NoError;
    //        }
    //        return res.ErrorCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
    //        await Helpers.Alerts.DisplaySnackbar(exceptionMessage);
    //        return ErrorCode.ExceptionError;
    //    }
    //}
}



















//var stopWatch = new Stopwatch();
//stopWatch.Start();
//stopWatch.Stop();
//timer.Text = stopWatch.Elapsed.Microseconds.ToString();