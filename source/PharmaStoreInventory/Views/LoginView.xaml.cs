using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Dtos.UserDtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.Views.Admin;

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

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        password.InputText = string.Empty;
    }

    private async void SubmitClicked(object sender, EventArgs e)
    {
        try
        {
            Result<UserLoginResponseDto>? res = null;
            Alerts.DisplayActivityIndicator(this);
            InputType check = CheckInputs();

            if (check == InputType.Empty)
            {
                Alerts.CloseActivityIndicator();
                return;
            }

            var userModel = new UserLoginRequestDto()
            {
                EmailOrPhone = email.InputText,
                Password = password.InputText,
                IsNewDevice = false,
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
            else if (check == InputType.Admin)
            {
                var adminModel = new LoginDto(email.InputText, password.InputText);
                res = await ApiServices.AdminLoginByEmailAsync(adminModel);
            }

            if (res == null)
            {
                _ = Alerts.DisplaySnackbar("Something went wrong");
                Alerts.CloseActivityIndicator();
                return;
            }

            // Navigation
            _ = Alerts.DisplaySnackbar(res.Message);
            if (res.IsSuccess)
            {
                // Save user data in json file
                if (res.Data != null)
                {
                    AppPreferences.HostUserId = res.Data.Id;
                    AppPreferences.IsLoggedIn = true;
                    AppPreferences.IsUserActivated = true;
                    _ = jsonFileHanler.WriteToFile(res.Data);
                }

                // Navigation
                if (check == InputType.Admin)
                {
                    await Navigation.PushAsync(new UsersView());
                }
                else if (AppPreferences.HasBranchRegistered)
                {
                    await Navigation.PushAsync(new DashboardView());
                }
                else
                {
                    await Navigation.PushAsync(new BranchesView());
                }
            }
            else
            {
                if (res.ErrorCode == ErrorCode.UserNotActive)
                {
                    await Navigation.PushAsync(new WaitingApprovalView());
                }
                AppPreferences.IsLoggedIn = true;
                AppPreferences.IsUserActivated = false;
            }

            //end try
            Alerts.CloseActivityIndicator();
            Navigation.RemovePage(this);
        }

        catch
        {
        }

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
            else if (email.InputText == "admin")
            {
                inputType = InputType.Admin;

            }
        }

        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            inputType = InputType.Empty;
        }

        return inputType;
    }// end CheckInputs

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterView());
        Navigation.RemovePage(this);
    }
}