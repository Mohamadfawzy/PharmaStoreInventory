using BarcodeScanning;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Views;
using System.Globalization;

namespace PharmaStoreInventory;
public partial class App : Application
{
    //public static AppDb Context = null!;
    public App()
    {
        UserAppTheme = AppTheme.Light;
        InitializeComponent();
        SetCulture("ar");
        HandlePreferencesKeys();
        IsDevelopmentMode();


        if (AppPreferences.IsFirstTime)
        {
            MainPage = new NavigationPage(new OnbordingView());
        }
        else if (!AppPreferences.IsLoggedIn)
        {
            MainPage = new NavigationPage(new LoginView());
        }
        else if (!AppPreferences.HasBranchRegistered)
        {
            MainPage = new NavigationPage(new BranchesView(false));
        }
        else
        {
            MainPage = new NavigationPage(new DashboardView());
        }

        //MainPage = new NavigationPage(new OnbordingView ());
    }

    private void IsDevelopmentMode()
    {
#if DEBUG
        AppValues.HostBaseURI = "http://192.168.1.103:5219/api";
        AppValues.IsDevelopment = true;
#endif
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
        CultureInfo culture = CultureInfo.GetCultureInfo("ar");

    }
    public void SetCulture(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);

        AppValues.Language = cultureCode;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
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
