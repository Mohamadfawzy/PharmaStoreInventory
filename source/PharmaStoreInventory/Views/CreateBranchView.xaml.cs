using DataAccess.Helper;
using DataAccess.Repository;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Views.Templates;
namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage
{
    //private Branch? branch = null;
    private readonly FileHanler fileHanler;
    public CreateBranchView()
    {
        InitializeComponent();
        fileHanler = new(Helpers.AppValues.BranchsFileName);

    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void SubmitButton(object sender, EventArgs e)
    {
        Helpers.Alerts.DisplayActivityIndicator(this);

        submitButton.IsEnabled = false;

        if (!DataValidaityCheck())
            return;

        var branch = new Branch()
        {
            Id = Guid.NewGuid(),
            BrachName = brachName.InputText,
            Password = password.InputText,
            Telephone = telephone.InputText,
            Username = username.InputText,
            IpAdrress = ipAdrress.InputText,
            Port = port.InputText
        };
        var status = await Connection(branch);
        if (status == ErrorType.Success)
        {
            await Helpers.Alerts.DisplaySnackbar("The branch has been contacted successfully");
            await fileHanler.Add(branch);
            Helpers.AppPreferences.HasBranchRegistered = true;


            //await Dispatcher.DispatchAsync(() =>
            //{
            //});
            await Navigation.PushAsync(new BranchesView());
        }
        else if (status == ErrorType.ConnectionString)
        {
            await Helpers.Alerts.DisplaySnackbar("IP or Port is Incorrect");
            //await Dispatcher.DispatchAsync(async () =>
            //{
            //});
        }
        else
        {
            await Helpers.Alerts.DisplaySnackbar("Username or Password is Incorrect");
            //await Dispatcher.DispatchAsync(() =>
            //{
            //});
        }

        ClosePopup();
        submitButton.IsEnabled = true;
    }

    private async Task<ErrorType> Connection(Branch branch)
    {
        try
        {
            Strings.IP = AppPreferences.IP = branch.IpAdrress;
            Strings.Port = AppPreferences.Port = branch.Port;

            var repo = new EmployeeRepo();
            var result = await repo.Login(branch.Username, branch.Password);
            if (result != null)
            {
                AppPreferences.LocalDbUserId = result.Id;
                return ErrorType.Success;
            }

            else
                return ErrorType.UsernameOrPass;
        }
        catch (Exception)
        {
            return ErrorType.ConnectionString;
        }
    }

    private bool DataValidaityCheck()
    {
        // if validation contentnio ...

        return true;
    }

    public void DisplayPopup()
    {
        //popup = new PopupWin();
        //await this.ShowPopupAsync(popup);
        Helpers.Alerts.DisplayActivityIndicator(this);
    }

    public void ClosePopup()
    {
        //await popup.CloseAsync();
        Helpers.Alerts.CloseActivityIndicator();
    }

    private enum ErrorType
    {
        Success,
        Username,
        Password,
        UsernameOrPass,
        ConnectionString
    }





























    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {


        //Device.BeginInvokeOnMainThread(() =>
        //{
        //    var t1 = AddElement1();
        //    var t2 = AddElement2();
        //    var t3 = AddElement3();
        //    var t4 = AddElement4();
        //    var t5 = AddElement5();
        //    var t6 = AddElement6();

        //    Task.WhenAll(t1, t2, t3, t4);
        //});
        //inputsContainer.IsVisible = true;
        //refresh.IsRefreshing = false;

    }
    /*

    private Task AddElement1()
    {
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 1" };
        inputsContainer.Add(a1);
        return Task.CompletedTask;
    }
    private Task AddElement2()
    {
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 2" };
        inputsContainer.Add(a1);
        return Task.CompletedTask;
    }

    private Task AddElement3()
    {
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 3" };
        inputsContainer.Add(a1);
        return Task.CompletedTask;
    }
    private async Task AddElement4()
    {
        await Task.Delay(1000);
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 4" };
        inputsContainer.Add(a1);
        //return Task.CompletedTask;
    }

    private async Task AddElement5()
    {
        await Task.Delay(1100);
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 5" };
        inputsContainer.Add(a1);
        //return Task.CompletedTask;
    }

    private async Task AddElement6()
    {
        await Task.Delay(1200);
        AnimatedInput a1 = new AnimatedInput() { InputPlaceholder = "Filed 6" };
        inputsContainer.Add(a1);
        //return Task.CompletedTask;
    }
    */
}



