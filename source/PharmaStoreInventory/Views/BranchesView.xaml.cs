using DataAccess.DomainModel;
using DataAccess.Services;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
namespace PharmaStoreInventory.Views;
public partial class BranchesView : ContentPage
{
    private readonly XmlFileHandler xFileHanler;
    public NoDataModel NoDataModel =>
        new("nodataicon.png", "لايوجد فروع مسجلة", "يجب تسجيل فرع واحد علي الأقل", false);
    public BranchesView(bool hasBackButton = true)
    {
        InitializeComponent();
        xFileHanler = new(AppValues.XBranchesFileName);
        GetAllBranchs();

        if (!hasBackButton)
        {
            backArrowIcon.IsVisible = false;
        }
    }

    //protected override bool OnBackButtonPressed()
    //{
    //    Navigation.PushAsync(new DashboardView());
    //    return true;

    //}

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    var count = Navigation.NavigationStack.Count;
    //    if (count == 1)
    //    {
    //        backArrowIcon.IsVisible = false;
    //    }
    //}

    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ConnectionClicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn == null)
            return;
        try
        {

            activityIndicator.IsRunning = true;
            btn.IsEnabled = false;

            var branch = btn.CommandParameter as BranchModel;
            if (branch != null)
            {
                var (status, message) = await ApiServices.ApiEmployeeLogin(branch);
                if (status == ConnectionErrorCode.Success)
                {
                    notification.Display("Contacted successfully", "The branch has been contacted successfully");
                    AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);
                    AppPreferences.HasBranchRegistered = true;
                    if (App.Current != null)
                        App.Current.MainPage = new NavigationPage(new DashboardView());
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
                //GetAllBranchs();
            }
        }
        catch
        {
            notification.Display("Something went wrong", "Exception");
        }
        finally
        {
            btn.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

    }

    private async void GetAllBranchs()
    {
        activityIndicator.IsRunning = true;

        var branches = await xFileHanler.All();

        if (branches == null || branches.Count < 1)
        {
            var list = await ApiServices.GetAllBranches(AppPreferences.HostUserId);
            if (list != null)
            {
                foreach (var branch in list)
                {
                    await xFileHanler.Add(branch);
                }
                branches = list;
            }
        }

        if (branches != null && branches.Count > 0)
        {
            clBranches.ItemsSource = branches;
            noDataTemplate.IsVisible = false;
        }
        else
        {
            noDataTemplate.IsVisible = true;
        }
        activityIndicator.IsRunning = false;
    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var item = btn.CommandParameter as BranchModel;
            if (item != null)
            {
                await xFileHanler.RemoveById(item.Id.ToString());
                await ApiServices.DeleteBranche(item.Id);
                GetAllBranchs();

                var url = await Configuration.ConfigureBaseUrl(item.IpAddress, item.Port);
                if (url == AppValues.LocalBaseURI)
                {
                    AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = string.Empty;
                    AppPreferences.HasBranchRegistered = false;
                    await Alerts.DisplayToast("قمت بحذف الفرع الحالي");
                    if (Application.Current != null)
                        Application.Current.MainPage = new NavigationPage(new BranchesView(false));
                }
            }
        }
    }

    private async void GotoCreateBranchTapped(object sender, TappedEventArgs e)
    {
        activityIndicator.IsRunning = true;
        await Navigation.PushAsync(new CreateBranchView());
        activityIndicator.IsRunning = false;

    }

    private async void NavigationPop_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}