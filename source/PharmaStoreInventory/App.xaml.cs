using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Views;
using System.Globalization;

namespace PharmaStoreInventory;
public partial class App : Application
{
    //public static AppDb Context = null!;
    public App()
    {
        //Context = new AppDb();
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
        AppValues.Language = "ar";
        GetCulture();
        HandlePreferencesKeys();
        if (AppPreferences.IsFirstTime)
        {
            MainPage = new NavigationPage(new OnbordingView());
        }
        else if (!AppPreferences.IsLoggedIn)
        {
            MainPage = new NavigationPage(new LoginView());
        }
        //else if(AppPreferences.IsLoggedIn && !AppPreferences.IsUserActivated)
        //{
        //    MainPage = new NavigationPage(new WaitingApprovalView());
        //}
        else if (!AppPreferences.HasBranchRegistered)
        {
            //MainPage = new NavigationPage(new CreateBranchView());
            MainPage = new NavigationPage(new BranchesView(false));
        }
        else
        {
            MainPage = new NavigationPage(new DashboardView());
        }
    }

#pragma warning disable CA1822
    void GetCulture()
    {
        //var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        //if (lang == "ar" || lang == "en" || lang == "fr")
        //    AppValues.Language = lang;
        //else
        //    AppValues.Language = "en";

        //CultureInfo culture = CultureInfo.GetCultureInfo("en-US");

    }

    void HandlePreferencesKeys()
    {
        try
        {
            //DataAccess.Helper.Strings.Port = AppPreferences.Port;
            //DataAccess.Helper.Strings.IP = AppPreferences.IP;
            AppValues.LocalBaseURI = AppPreferences.LocalBaseURI;
            AppValues.LeftScanIcon = AppPreferences.LeftScanIcon;
            AppValues.ProductHasQuantityOnly = AppPreferences.ProductHasQuantityOnly;
        }
        catch
        {
        }
    }
#pragma warning restore CA1822

}
