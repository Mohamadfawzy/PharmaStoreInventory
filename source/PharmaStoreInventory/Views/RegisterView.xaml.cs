using CommunityToolkit.Maui.Views;
using DataAccess.DomainModel;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.ViewModels;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage
{
    readonly RegisterViewModel vm;
    private Popup popup;
    public RegisterView()
    {
        InitializeComponent();
        popup = new PopupWin();
        vm = (RegisterViewModel)BindingContext;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //email.HideKeyBoard();
        inputsContainer.ClearFocusFromAllInputs();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (ThisPage.Height > 100)
        {
            this.HeightRequest = ThisPage.Height + 400;

        }
    }
    private async void SubmitClicked(object sender, EventArgs e)
    {
        //var stopWatch = new Stopwatch();
        //stopWatch.Start();


        Helpers.Alerts.DisplayActivityIndicator(this);
        var isError = CheckInputs();
        if (!isError)
        {
            var res = await vm.CheckEmailValidityAndSendEmail();

            if (res == ErrorCode.EmailAlreadyExists)
            {
                email.IsError = true;
                email.ErrorMessage = "الايميل مسجل من قبل";
            }
            else if (res == ErrorCode.PhoneNumberAlreadyExists)
            {
                telephone.IsError = true;
                telephone.ErrorMessage = "رقم الهاتف مسجل من قبل ";
            }
            else
                await Helpers.Alerts.DisplaySnackbar("ExceptionError");

        }
        Helpers.Alerts.CloseActivityIndicator();

        //stopWatch.Stop();
        //timer.Text = stopWatch.Elapsed.Microseconds.ToString();
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
    }// end CheckInputs


    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // when releas remove this
        inputsContainer.PositioningOfPlaceHolder();
        var verfViewTemplate = new VerificationViewTemplate() { ZIndex = 2 };
        manStack.SetRowSpan(verfViewTemplate, 3);
        manStack.Add(verfViewTemplate);
    }
}