using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.Views;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly MailingService mailingService;
   // private readonly AuthService authService;


    public UserRegisterDto UserRegister { get; set; }

    public bool verificationViewTemplateVisible = false;
    public bool VerificationViewTemplateVisible
    {
        get => verificationViewTemplateVisible;
        set => SetProperty(ref verificationViewTemplateVisible, value);
    }
    private string verificationCodeSended = string.Empty;
    // ########################################################################
    public RegisterViewModel()
    {
        mailingService = new();
        UserRegister = new()
        {
            ConfirmNewPassword = "1",
            Password = "1",
            Email = "mohamedfawzy733@yahoo.com",
            FullName = "mohamed from mopile",
            PharmcyName = "fawzy pharm",
            PhoneNumber = "01093052428"
        }
        ;
        Helpers.AppPreferences.SetDeviceID();
        UserRegister.DeviceID = Helpers.AppPreferences.GetDeviceID();
        Code = new();
        ShowNotification(new Models.ErrorMessage("test", "") );
    }

    public VerificationCodeValues Code { get; set; }

    public ICommand SubmitCommand => new Command(ExecuteSubmit);
    public ICommand SendEmailCommand => new Command(ExecuteSendEmail);


    async void ExecuteSubmit()
    {
        var code = Code.Value1 + Code.Value2 + Code.Value3 + Code.Value4;
        if (code == verificationCodeSended)
        {
            CreateAcount();
            //VerificationViewTemplateVisible = false;

        }
    }

    async void ExecuteSendEmail()
    {
        //verificationCodeSent = await mailingService.SendVerificationCodeAsync(UserRegister.Email!, null, "mohamed fawzy");
    }
    private async Task CreateAcount()
    {
        try
        {
            var res = await ApiServices.RegisterUserAsync(UserRegister);
            if (res == null)
            {
                await Helpers.Alerts.DisplaySnackBar("RegisterUserAsync return null");
                return;
            }
            _ = Helpers.Alerts.DisplaySnackBar(res.Message);
            if (res.IsSuccess)
            {
                if (Application.Current != null && Application.Current.MainPage != null)
                {
                    var navigation = Application.Current.MainPage.Navigation;
                    var last = navigation.NavigationStack[navigation.NavigationStack.Count - 1];
                    await navigation.PushAsync(new LoginView());
                    if (last != null)
                    {
                        navigation.RemovePage(last);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
            await Helpers.Alerts.DisplaySnackBar(exceptionMessage);
        }
    }
    
    //public async Task<ErrorCode> CheckEmailValidityAndSendEmail()
    //{
    //    try
    //    {
    //        //cheack if email is not exist
    //        var res = await ApiServices.AreEmailAndPhoneNonExistentAsync(uRegister.Email!, uRegister.PhoneNumber!);
    //        //_ = Helpers.Alerts.DisplaySnackBar(res.Message);
    //        if (res == null)
    //            return ErrorCode.NullValue;

    //        if (res.IsSuccess)
    //        {
    //            VerificationViewTemplateVisible = true;
    //            verificationCodeSent = "1111";// await mailingService.SendVerificationCodeAsync(uRegister.Email!, null, "mohamed fawzy");
    //            return ErrorCode.NoError;
    //        }
    //        return res.ErrorCode;
    //    }
    //    catch (Exception ex)
    //    {
    //        var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
    //        await Helpers.Alerts.DisplaySnackBar(exceptionMessage);
    //        return ErrorCode.ExceptionError;
    //    }
    //}


    void ShowNotification(ErrorMessage error)
    {
        WeakReferenceMessenger.Default.Send(new RegisterViewNotification(error));
    }

}