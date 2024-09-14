using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;
namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage, IRecipient<CreateBranchViewotification>
{

    private readonly XmlFileHandler xFileHanler;
    private EmployeeDto? employee = null;

    public void Receive(CreateBranchViewotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.ShowMessage(message.Value);
        });
    }
    public CreateBranchView()
    {
        InitializeComponent();
        xFileHanler = new(AppValues.XBranchsFileName);
    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void CreateBranchClicked(object sender, EventArgs e)
    {
        activityIndicator.IsRunning = true;
        btnCreateBranch.IsEnabled = false;
        inputsContainer.ClearFocusFromAllInputs();

        if (AreEntriesValid())
        {
            var branch = new BranchModel()
            {
                //Id = Guid.Parse("e3b9f4f0-0e47-46ad-bd68-bf9587b85776"),// Guid.NewGuid(),
                Id = Guid.NewGuid(),
                BrachName = brachName.InputText,
                Telephone = telephone.InputText,
                IpAddress = ipAdrress.InputText,
                Port = port.InputText,
                Username = username.InputText,
                Password = password.InputText,
                UserId = AppPreferences.HostUserId,
            };
            CreateBranch(branch);
        }

        activityIndicator.IsRunning = false;
        btnCreateBranch.IsEnabled = true;
    }

    private bool AreEntriesValid()
    {
        var bools = new List<bool>(6);
        if (string.IsNullOrEmpty(brachName.InputText))
        {
            brachName.IsError = true;
            bools.Add(false);
        }
        if (string.IsNullOrEmpty(telephone.InputText) || telephone.IsError)
        {
            telephone.IsError = true;
            bools.Add(false);
        }
        if (string.IsNullOrEmpty(ipAdrress.InputText))
        {
            ipAdrress.IsError = true;
            bools.Add(false);
        }
        if (string.IsNullOrEmpty(port.InputText))
        {
            port.IsError = true;
            bools.Add(false);
        }
        if (string.IsNullOrEmpty(username.InputText))
        {
            username.IsError = true;
            bools.Add(false);
        }
        if (string.IsNullOrEmpty(password.InputText))
        {
            password.IsError = true;
            bools.Add(false);
        }

        if (bools.Count > 0)
        {
            return false;
        }
        return true;
    }

    async void CreateBranch(BranchModel branch)
    {
        // check if this bransh is already exist. if ture? We will not add it.
        if (await xFileHanler.IsIpAdrressExist(branch.IpAddress))
        {
            notification.ShowMessage("Failure", "Ip Adrress already Exist");
            return;
        }

        var (status, message) = await ApiServices.ApiEmployeeLogin(branch);
        if (status == ConnectionErrorCode.Success)
        {
            // 1 Save branch data on host
            var result = await ApiServices.AddBranche(branch);

            if (result != null && !result.IsSuccess)
            {
                notification.ShowMessage("حدث خطأ عن اضافة الفرع علي الهوست", result.Message);
                return;
            }

            notification.ShowMessage("Contacted successfully", "The branch has been contacted successfully");

            // 2 Save branch data locally
            var t1 = xFileHanler.Add(branch);

            // 3 Save Preferences
            var t2 = SetPreferences(branch);

            // 4 Navigation
            var t4 = Navigation.PushAsync(new BranchesView());
            await Task.WhenAll(t1, t2, t4);
        }
        else if (status == ConnectionErrorCode.Fail)
        {
            notification.ShowMessage("فشل في الاتصال بالسيرفر", "IP or Port is Incorrect");
        }
        else if (status == ConnectionErrorCode.UsernameOrPass)
        {
            notification.ShowMessage("فشل في الاتصال بالنظام", "username or Password is Incorrect");
        }
        else
        {
            notification.ShowMessage("Something went wrong", message);
        }
    }

    private async Task<ConnectionErrorCode> ApiEmployeeLogin(BranchModel branch)
    {
        try
        {
            //Strings.IP = AppPreferences.IP = branch.IpAddress;
            //Strings.Port = AppPreferences.Port = branch.Port;
            // http://192.168.1.103:5144/api
            //var repo = new EmployeeRepo();

            var uri = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);

            var emp = new LoginDto(branch.Username, branch.Password);
            var result = await ApiServices.EmpLogin(uri, emp);

            // status 1 Unable to connect to server
            if (result == null)
            {
                return ConnectionErrorCode.Fail;
            }
            // status 2 Server connection IsSuccess
            if (result != null && result.IsSuccess)
            {
                if (result.Data != null)
                {
                    employee = result.Data;
                }
                return ConnectionErrorCode.Success;
            }
            // status 3 Server connection IsSuccess, but username or password is incorrect
            else
                return ConnectionErrorCode.UsernameOrPass;
        }
        catch (Exception ex)
        {
            notification.ShowMessage("Something went wrong", ex.Message);
            return ConnectionErrorCode.Exception;
        }
    }

    async Task SetPreferences(BranchModel branch)
    {
        AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);
        AppPreferences.LocalDbUserId = employee != null ? employee.Id : 0;
        AppPreferences.HasBranchRegistered = true;
        //AppPreferences.EmpUsername = branch.Username;
        //AppPreferences.EmpPassword = branch.Password;
    }

    // will deleted
    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        brachName.InputText = "شبين الكوم";
        telephone.InputText = "0402555550";
        ipAdrress.InputText = "192.168.1.103";
        port.InputText = "5144";
        username.InputText = "admin";
        password.InputText = "admin";
        inputsContainer.PositioningOfPlaceHolder();
    }
}



