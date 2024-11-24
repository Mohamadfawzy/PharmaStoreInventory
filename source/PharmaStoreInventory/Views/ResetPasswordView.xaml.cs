using DataAccess.DomainModel;
using DataAccess.Helper;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class ResetPasswordView : ContentPage
{
    #region OnStart
    private EmailRequestModel emailModel = new();
    public ResetPasswordView()
    {
        InitializeComponent();
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

    private void NetworkNotAccessAlert()
    {
        notification.Display(new Models.ErrorMessage("No NetworkAccess", "please check your WiFi"));
    }

    void SetValues()
    {
        if (!string.IsNullOrEmpty(AppPreferences.UserEmail))
        {
            email.SetInputText(AppPreferences.UserEmail);
        }
    }
    #endregion

    #region OnClicked
    private async void MainButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // 1 validate inputs
            if (!ValidateInputs())
            {
                return;
            }

            // 2 fined the email in dada base
            await IsEmailExistAsync();

            // 3 show verification view
            ShowVerificationViewTemplate();

            //4 send email
            await IsSuccessSendEmailAsync();

        }
        catch (Exception ex)
        {
            notification.Display(ex.Message);
        }
    }

    private async void VerificationViewTemplate_SubmitClicked(object sender, EventArgs e)
    {
        // 5
        var res = await ApiServices.IsVerificationCodeValidAsync(emailModel.EVCID, verificationViewTemplate.GetCode());
        if (res != null && res.IsSuccess)
        {
            body.InputTransparent = true;
            restView.IsVisible = true;
        }
        //if (verificationCodeSent == verificationViewTemplate.GetCode())
        //{
        //    //notification.Display("تم التحقق من الإيميل");
        //    mainCreationButton.Text = "تأكيد إنشاء الحساب";
        //    //verificationViewTemplate.IsVisible = false;
        //    email.IsEnabled = false;
        //    //isEmailVerified = true;
        //    await CreateAccount(uRegister);
        //}
        else
        {
            notification.Display("الكود خاطئ");
        }
    }

    private async void ResetPasswordButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            if (IsAnyInvalidInput())
            {
                return;
            }

            var model = new ResetPasswordRequest()
            {
                EVCID = emailModel.EVCID,
                Email = emailModel.Recipient,
                Code = verificationViewTemplate.GetCode(),
                Password = password.InputText,
            };
            var res = await ApiServices.ResetPasswordAsync(model);
            if (res != null && res.IsSuccess)
            {
                body.InputTransparent = false;
                await Navigation.PopToRootAsync();
            }
            else
            {
                notification.Display("لم يتم التحديث الكود خاطئ");
            }

        }
        catch (Exception ex)
        {
            notification.Display(ex.Message);
        }
    }

    private async void NavigationPop_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
    #endregion

    #region On Call API
    async Task IsEmailExistAsync()
    {
        var res = await ApiServices.IsEmailExistAsync(email.InputText);

        if (res == null)
        {
            throw new Exception("تحقق من الاتصال بالسيرفر");
        }

        if (!res.IsSuccess)
        {
            email.SetError("البريد الإلكتروني غير صالح");
            notification.Display("تحقق من البريد الإلكتروني");
        }
    }
    async Task IsSuccessSendEmailAsync()
    {
        emailModel.Recipient = email.InputText;
        emailModel.EVCID = Guid.NewGuid();
        var res = await ApiServices.PostAndSendEmail(emailModel);
        if (res != null && res.IsSuccess)
        {
            notification.Display("تم ارسال الكود");
        }
        else
            notification.Display("لم يتم ارسال الكود");
    }
    #endregion

    #region On process
    bool ValidateInputs()
    {
        if (email.IsAnyError())
        {
            email.SetError("تحقق من صحة البيانات المدخلة");
            return false;
        }
        return true;
    }

    private void ShowVerificationViewTemplate()
    {
        verificationViewTemplate.IsVisible = true;
        verificationViewTemplate.Init();
        verificationViewTemplate.SetSpanEmail(email.InputText);
    }

    bool IsAnyInvalidInput()
    {
        var enyErrorFound = false;
        if (password.IsAnyError())
        {
            enyErrorFound = true;
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