using DataAccess.Dtos.UserDtos;
using DataAccess.Repository;
using DataAccess.Services;
namespace PharmaStoreInventory.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly MailingService mailingService;
    private readonly AuthService authService;


    public UserRegisterDto UserRegister { get; set; }
    //public bool ShowError
    //{
    //    get => showError;
    //    set => SetProperty(ref showError, value);
    //}

    // ########################################################################
    public RegisterViewModel()
    {
        var repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<UserRepository>();
        authService = new(repo);
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
        UserRegister.DeviceID =  Helpers.AppPreferences.GetDeviceID();
    }

    public async Task<bool> SubmitExecute()
    {
        try
        {

            var result = await mailingService.SendVerificationCodeAsync(UserRegister.Email!, null, "mohamed fawzy");
            Helpers.AppConstants.VerificationCode = result;
            var res = await authService.RegisterUserAcync(UserRegister);
            _ = Helpers.Alerts.DisplaySnackbar(res.Message);
            if (res.IsSuccess)
            {
                return true;
            }
            return false;
            //App.Current?.MainPage?.Navigation.PushAsync(new LoginView());
        }
        catch (Exception ex)
        {
            var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
            await Helpers.Alerts.DisplaySnackbar(exceptionMessage);
            return false;
        }
    }


}