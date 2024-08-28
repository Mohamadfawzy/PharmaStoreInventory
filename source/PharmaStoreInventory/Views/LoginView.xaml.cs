using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        try
        {
            Result<UserLoginResponseDto>? res;
            Helpers.Alerts.DisplayActivityIndicator(this);
            InputType check = CheckInputs();

            if (check == InputType.Empty)
            {
                Helpers.Alerts.CloseActivityIndicator();
                return;
            }

            var loginModel = new UserLoginRequestDto()
            {
                EmailOrPhone = email.InputText,
                Password = password.InputText,
                IsNewDevice = false,
                DviceId = Helpers.AppPreferences.GetDeviceID()
            };

            if (check == InputType.Phone)
            {
                res = await ApiServices.UserLoginByPhoneAsync(loginModel);
            }
            else // if( input == Email?
            {
                res = await ApiServices.UserLoginByEmailAsync(loginModel);
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