using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Repository;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Validations;

namespace PharmaStoreInventory.Views;

public partial class LoginView : ContentPage
{
    //private AuthService authService;
    public LoginView()
    {
        InitializeComponent();
    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {

    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        //var repo = Handler?.MauiContext?.Services.GetService<UserRepository>();
        //repo ??= new UserRepository(new DataAccess.Contexts.AppHost());


        var repo = new UserRepository(new DataAccess.Contexts.AppHost());
        
        AuthService authService = new(repo);
        try
        {
            Result<UserLoginResponseDto> res;
            Helpers.Alerts.DisplayActivityIndicator(this);
            InputType check = CheckInputs();

            if (check == InputType.Empty)
            {
                Helpers.Alerts.CloseActivityIndicator();
                return;
            }

            if (check == InputType.Phone)
            {
                res = await authService.UserLoginByPhoneAsync(email.InputText, password.InputText, Helpers.AppPreferences.GetDeviceID(), false);
            }
            else // if( input == Email?
            {
                res = await authService.UserLoginByEmailAsync(email.InputText, password.InputText, Helpers.AppPreferences.GetDeviceID(), false);
            }

            if (res == null)
            {
                _ = Helpers.Alerts.DisplaySnackbar("Something went wrong");
                Helpers.Alerts.CloseActivityIndicator();
                return;
            }


            // navigation
            _ = Helpers.Alerts.DisplaySnackbar(res.Message);
            if (res.IsSuccess)
            {

                if (Helpers.AppPreferences.HasBranchRegistered)
                {
                    await Navigation.PushAsync(new DashboardView());
                    Navigation.RemovePage(this);
                }
                else
                {
                    await Navigation.PushAsync(new CreateBranchView());
                    Navigation.RemovePage(this);
                }
                Helpers.AppPreferences.IsLoggedIn = true;
                Helpers.AppPreferences.HostUserId = res.Data != null ? res.Data.Id : 0;
            }
            else
            {
                if (res.ErrorCode == ErrorCode.UserNotActive)
                {
                    await Navigation.PushAsync(new WaitingApprovalView());
                }
            }

            //end try
            Helpers.Alerts.CloseActivityIndicator();
        }

        catch
        {
        }

    }

    private InputType CheckInputs()
    {
        InputType isError = InputType.Empty;

        if (string.IsNullOrEmpty(email.InputText))
        {
            email.IsError = true;
        }
        else
        {
            if (DataAccess.DomainModel.Validator.IsValidTelephone(email.InputText))
            {
                isError = InputType.Phone;
            }
            else if (DataAccess.DomainModel.Validator.IsValidEmail(email.InputText))
            {
                isError = InputType.Email;
            }
        }

        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            isError = InputType.Empty;
        }

        return isError;
    }// end CheckInputs
}