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
        notification.ShowMessage(new Models.ErrorMessage("No NetworkAccess", "please check your WiFi"));
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

        // validate inputs
        // fined the email in dada base
        // show verification view
        // send email
        try
        {
            // 1
            if (!ValidateInputs())
            {
                return;
            }

            // 2
            await IsEmailExistAsync();

            // 3
            ShowVerificationViewTemplate();

            //4
            await IsSuccessSendEmailAsync();

        }
        catch (Exception ex)
        {
            notification.ShowMessage(ex.Message);
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
        //    //notification.ShowMessage("تم التحقق من الإيميل");
        //    mainCreationButton.Text = "تأكيد إنشاء الحساب";
        //    //verificationViewTemplate.IsVisible = false;
        //    email.IsEnabled = false;
        //    //isEmailVerified = true;
        //    await CreateAccount(uRegister);
        //}
        else
        {
            notification.ShowMessage("الكود خاطئ");
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
                Code = emailModel.VerificationCode,
                Password = password.InputText,
            };
            var res = await ApiServices.ResetPasswordAsync(model);
            if (res != null && res.IsSuccess)
            {
                body.InputTransparent = false;
                await Navigation.PopToRootAsync();
            }

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {

        }
    }

    private async void NavigationPop_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
    #endregion

    bool ValidateInputs()
    {
        if (email.IsAnyError())
        {
            email.SetError("تحقق من صحة البيانات المدخلة");
            return false;
        }
        return true;
    }

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
            notification.ShowMessage("تحقق من البريد الإلكتروني");
        }
        //else
        //{
        //    notification.ShowMessage("البريد الإلكتروني صالح!");
        //}
    }

    private void ShowVerificationViewTemplate()
    {
        verificationViewTemplate.IsVisible = true;
        verificationViewTemplate.Init();
        verificationViewTemplate.SetSpanEmail(email.InputText);
    }

    async Task IsSuccessSendEmailAsync()
    {
        emailModel.Recipient = email.InputText;
        emailModel.VerificationCode = Common.GenerateVerificationCode();
        emailModel.EVCID = Guid.NewGuid();
        //Debug.WriteLine("\n\n\n\n\n\n\n Guid: "+emailModel.EVCID);
        var res = await ApiServices.PostAndSendEmail(emailModel);
        if (res != null && res.IsSuccess)
        {
            notification.ShowMessage("تم ارسال الكود");
        }
        else
            notification.ShowMessage("لم يتم ارسال الكود");
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


}