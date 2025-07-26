using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Services;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Services;
namespace PharmaStoreInventory.Views;

public partial class CreateBranchView : ContentPage, IRecipient<CreateBranchViewNotification>
{
    private readonly XmlFileHandler xFileHandler;

    #region OnStart
    public CreateBranchView()
    {
        InitializeComponent();
        xFileHandler = new(AppValues.XBranchesFileName);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        Microsoft.Maui.Controls.Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                 .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
#endif
        GetPharmaVersion();
    }

    public void Receive(CreateBranchViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.Display(message.Value);
        });
    }
    #endregion

    #region OnClicked
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

            if (!AreEntriesValid())
            {
                return;
            }

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
        catch (Exception ex)
        {
            notification.Display("exception in event clicked", ex.Message);
        }
        finally
        {
            activityIndicator.IsRunning = false;
            btnCreateBranch.IsEnabled = true;
        }
    }
    private void SetInputText_Tapped(object sender, TappedEventArgs e)
    {
        if (AppValues.IsDevelopment)
        {
            brachName.SetInputText("اسم الفرع");
            telephone.SetInputText("0402555550");
            ipAdrress.SetInputText("192.168.1.103");
            port.SetInputText("6003");
            username.SetInputText("111");
            password.SetInputText("111");
        }
    }
    #endregion

    #region On Call API
    private async Task CreateBranch(BranchModel branch)
    {
        // check if this branch is already exist. if true? We will not add it.
        if (await xFileHandler.IsIpAdrressExist(branch.IpAddress))
        {
            notification.Display("Failure", "Ip Address already Exist");
            return;
        }

        var (status, message) = await ApiServices.ApiEmployeeLogin(branch, AppPreferences.UserEmail, AppPreferences.UserPassword);
        if (status == ConnectionErrorCode.Success)
        {
            // 1 Check version compatibility
            await GetSystemVersion(await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port));
            if (AppValues.SystemVersion != AppValues.PharmaVersion)
            {
                notification.Display("خطأ في الإصدار أو الصلاحية", "من فضلك تحقق من توافق الإصدار ومن صلاحية المستخدم");
                return;
            }

            // 2 Save branch data on ModernSoft host
            var result = await ApiServices.AddBranche(branch);
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    notification.Display("خطأ  علي الهوست", result.Message);
                    return;
                }
            }
            else
            {
                notification.Display("حدث خطأ عن اضافة الفرع علي الهوست", "تحقق من الأتصال بالانترنت");
                return;
            }


            // 3 Toast Message
            await Alerts.DisplayToast("Contacted successfully");

            // 4 Save branch data locally
            var t1 = xFileHandler.Add(branch);

            // 5 Save Preferences
            var t2 = SetPreferences(branch);

            // 6 Navigation
            var t4 = Navigation.PushAsync(new BranchesView());
            await Task.WhenAll(t1, t2, t4);
        }
        else if (status == ConnectionErrorCode.Fail)
        {
            notification.Display("فشل في الاتصال بالسيرفر", "IP or Port is Incorrect");
        }
        else if (status == ConnectionErrorCode.UsernameOrPass)
        {
            notification.Display("فشل في الاتصال بالنظام", "username or Password is Incorrect");
        }
        else
        {
            notification.Display("Something went wrong", message);
        }
    }

    private async void GetPharmaVersion()
    {
        var pharmaVersion = await ApiServices.GetCurrentPharmaVersionAsync();

        if (pharmaVersion != null && pharmaVersion.VersionName != null)
        {
            AppValues.PharmaVersion = pharmaVersion.VersionName;
        }
    }

    private async Task GetSystemVersion(string url)
    {
        var systemVersion = await ApiServices.GetCurrentSystemVersionAsync(url);

        if (systemVersion != null && systemVersion.Ver_code != null)
        {
            AppValues.SystemVersion = systemVersion.Ver_code;
        }

    }
    #endregion

    #region On process
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
    private async Task SetPreferences(BranchModel branch)
    {
        AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);
        AppPreferences.HasBranchRegistered = true;
    }
    #endregion
}



