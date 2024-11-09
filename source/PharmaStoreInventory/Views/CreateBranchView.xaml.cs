using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Services;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;
namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage, IRecipient<CreateBranchViewNotification>
{

    private readonly XmlFileHandler xFileHandler;
    //private EmployeeDto? employee = null;

    public void Receive(CreateBranchViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.ShowMessage(message.Value);
        });
    }
    public CreateBranchView()
    {
        InitializeComponent();
        xFileHandler = new(AppValues.XBranchesFileName);
    }

    private void ClearFocusFromAllInputsTapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private async void CreateBranchClicked(object sender, EventArgs e)
    {
        try
        {
            activityIndicator.IsRunning = true;
            btnCreateBranch.IsEnabled = false;
            inputsContainer.ClearFocusFromAllInputs();

            if (AreEntriesValid())
            {
                var branch = new BranchModel()
                {
                    Id = Guid.NewGuid(),
                    BrachName = brachName.InputText,
                    Telephone = telephone.InputText,
                    IpAddress = ipAdrress.InputText,
                    Port = port.InputText,
                    Username = username.InputText,
                    Password = password.InputText,
                    UserId = AppPreferences.HostUserId,
                };
                await CreateBranch(branch);
            }
        }
        catch (Exception ex)
        {
            notification.ShowMessage("exception in event clicked", ex.Message);
        }
        finally
        {
            activityIndicator.IsRunning = false;
            btnCreateBranch.IsEnabled = true;
        }
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

    private async Task CreateBranch(BranchModel branch)
    {
        // check if this branch is already exist. if true? We will not add it.
        if (await xFileHandler.IsIpAdrressExist(branch.IpAddress))
        {
            notification.ShowMessage("Failure", "Ip Address already Exist");
            return;
        }

        var (status, message) = await ApiServices.ApiEmployeeLogin(branch);
        if (status == ConnectionErrorCode.Success)
        {
            // 1 Save branch data on ModernSoft host
            var result = await ApiServices.AddBranche(branch);
            if (result == null)
            {
                notification.ShowMessage("حدث خطأ عن اضافة الفرع علي الهوست");
                return;
            }

            if(result != null && !result.IsSuccess)
            {
                notification.ShowMessage("لم يتم اضافة الرفع",result.Message);
                return;
            }


            // 2 Toast Message
            await Alerts.DisplayToast("Contacted successfully");

            // 3 Save branch data locally
            var t1 = xFileHandler.Add(branch);

            // 4 Save Preferences
            var t2 = SetPreferences(branch);

            // 5 Navigation
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

    private async Task SetPreferences(BranchModel branch)
    {
        AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);
        AppPreferences.HasBranchRegistered = true;
        //AppPreferences.LocalDbUserId = employee != null ? employee.Id : 0;
        //AppPreferences.EmpUsername = branch.Username;
        //AppPreferences.EmpPassword = branch.Password;
    }

    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        if (AppValues.IsDevelopment)
        {
            brachName.SetInputText("اسم الفرع");
            telephone.SetInputText("0402555550");
            ipAdrress.SetInputText("192.168.1.103");
            port.SetInputText("5145");
            username.SetInputText("admin");
            password.SetInputText("admin");
        }
    }

    // Deleted
    //private async Task<ConnectionErrorCode> ApiEmployeeLogin(BranchModel branch)
    //{
    //    try
    //    {
    //        //Strings.IP = AppPreferences.IP = branch.IpAddress;
    //        //Strings.Port = AppPreferences.Port = branch.Port;
    //        // http://192.168.1.103:5144/api
    //        //var repo = new EmployeeRepo();

    //        var uri = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);

    //        var emp = new LoginDto(branch.Username, branch.Password);
    //        var result = await ApiServices.EmpLogin(uri, emp);

    //        // status 1 Unable to connect to server
    //        if (result == null)
    //        {
    //            return ConnectionErrorCode.Fail;
    //        }
    //        // status 2 Server connection IsSuccess
    //        if (result != null && result.IsSuccess)
    //        {
    //            if (result.Data != null)
    //            {
    //                employee = result.Data;
    //            }
    //            return ConnectionErrorCode.Success;
    //        }
    //        // status 3 Server connection IsSuccess, but username or password is incorrect
    //        else
    //            return ConnectionErrorCode.UsernameOrPass;
    //    }
    //    catch (Exception ex)
    //    {
    //        notification.ShowMessage("Something went wrong", ex.Message);
    //        return ConnectionErrorCode.Exception;
    //    }
    //}
}



