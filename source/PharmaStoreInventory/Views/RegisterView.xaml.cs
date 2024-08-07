using CommunityToolkit.Maui.Views;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.ViewModels;
using PharmaStoreInventory.Views.Templates;
using System;
using System.Diagnostics;

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
    //short count=0;
    //protected override bool OnBackButtonPressed()
    //{
    //    if (count == 0)
    //    {
    //        count = 1;
    //        inputsContainer.ClearFocusFromAllInputs();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
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
            var res = await vm.SubmitExecute();
            if (res)
            {
                await Navigation.PushAsync(new LoginView());
                Navigation.RemovePage(this);
            }
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
            isError =true;
        }
        return isError;
    }// end CheckInputs
}