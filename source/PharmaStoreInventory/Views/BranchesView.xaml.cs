using DataAccess.DomainModel;
using DataAccess.Services;
using Microsoft.Maui.Controls;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
namespace PharmaStoreInventory.Views;
public partial class BranchesView : ContentPage
{
    readonly XmlFileHandler xFileHanler;
    public BranchesView()
    {
        InitializeComponent();
        xFileHanler = new(AppValues.XBranchsFileName);
        GetAllBranchs();
    }
    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void ConnectionClicked(object sender, EventArgs e)
    {

        var btn = sender as Button;
        if (btn == null)
        {
            return;
        }
        activityIndicator.IsRunning = true;
        btn.IsEnabled = false;

        var branch = btn.CommandParameter as BranchModel;
        if (branch != null)
        {
            var (status, message) = await ApiServices.ApiEmployeeLogin(branch);
            if (status == ConnectionErrorCode.Success)
            {
                notification.ShowMessage("Contacted successfully", "The branch has been contacted successfully");
                AppPreferences.LocalBaseURI = AppValues.LocalBaseURI = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);
                AppPreferences.HasBranchRegistered = true;
                App.Current.MainPage = new NavigationPage(new DashboardView());
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
            GetAllBranchs();
        }


        btn.IsEnabled = true;
        activityIndicator.IsRunning = false;
    }

    protected override bool OnBackButtonPressed()
    {
        Navigation.PushAsync(new DashboardView());
        return true;

    }

    private async void GetAllBranchs()
    {
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
        clBranches.ItemsSource = branches;

        if(branches != null)
        {
            noDataTemplate.IsVisible = false;
        }
        else
            noDataTemplate.IsVisible = true;
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
            }
        }
    }

    private async void GotoCreateBranchClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateBranchView());
    }
}