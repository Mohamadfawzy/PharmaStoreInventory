using CommunityToolkit.Maui.Views;
using DataAccess;
using DataAccess.Helper;
using DataAccess.Repository;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Views.Templates;
namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage
{
    private enum ErrorType
    {
        Success,
        Username,
        Password,
        UsernameOrPass,
        ConnectionString
    }

    private EmployeeRepo repo;
    private AppDb context;
    private Branch branch;
    private Popup popup;
    private FileHanler FileHanler;
    public CreateBranchView()
    {
        InitializeComponent();
        FileHanler = new();
    }
    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }
    private async void SubmitButton(object sender, EventArgs e)
    {
        DisplayPopup();
        submitButton.IsEnabled = false;

        if (!DataValidaityCheck())
            return;

        var status = await Task.Run(TestConnection);
        if (status == ErrorType.Success)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                Helpers.CatchingException.DisplaySnackbar("The branch has been contacted successfully");
                SaveDataLocaly();
            });
            await Navigation.PushAsync(new BranchesView());
        }
        else if (status == ErrorType.ConnectionString)
        {
            await Dispatcher.DispatchAsync(() =>
            {
                Helpers.CatchingException.DisplaySnackbar("IP or Port is Incorrect");
            });
        }
        else
        {
            await Dispatcher.DispatchAsync(() =>
            {
                Helpers.CatchingException.DisplaySnackbar("Username or Password is Incorrect");
            });
        }

        ClosePopup();
        submitButton.IsEnabled = true;
    }

    private async Task<ErrorType> TestConnection()
    {
        try
        {
            DataAccess.Helper.Constants.IP = branch.IpAdrress;
            DataAccess.Helper.Constants.Port = branch.Port;

            context = new AppDb();
            repo = new(context);

            var result = await repo.Login(branch.Username, branch.Password);
            if (result != null)
                return ErrorType.Success;

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
        branch = new Branch()
        {
            Id = Guid.NewGuid(),
            BrachName = brachName.InputText,
            Password = password.InputText,
            Telephone = telephone.InputText,
            Username = username.InputText,
            IpAdrress = ipAdrress.InputText,
            Port = port.InputText
        };
        return true;
    }

    public async void DisplayPopup()
    {
        popup = new PopupWin();
        await this.ShowPopupAsync(popup);
    }

    public async void ClosePopup()
    {
        await popup.CloseAsync();
    }

    async void SaveDataLocaly()
    {
        await FileHanler.Add(branch, Helpers.AppConstants.BranchsFileName);
    }










    private async void TestConnectionOld()
    {
        DataAccess.Helper.Constants.IP = branch.IpAdrress;
        DataAccess.Helper.Constants.Port = branch.Port;

        context = new AppDb();
        repo = new(context);

        var result = await repo.Login(branch.Username, branch.Password);
        if (result != null)
        {

            await Dispatcher.DispatchAsync(() =>
            {
                Helpers.CatchingException.DisplaySnackbar("");
                ClosePopup();
            });
        }
    }

    void SetPreferencesKeys()
    {
        try
        {
            bool hasKey = Preferences.Default.ContainsKey("Port");
            if (hasKey)
            {
                DataAccess.Helper.Constants.Port = Preferences.Default.Get("Port", "1344");
                DataAccess.Helper.Constants.IP = Preferences.Default.Get("IP", "192.168.1.103");
            }
        }
        catch (Exception)
        {
        }
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

}