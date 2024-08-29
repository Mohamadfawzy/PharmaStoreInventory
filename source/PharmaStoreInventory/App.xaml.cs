using DataAccess.Contexts;
using Microsoft.Maui.Storage;
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
        InitializeComponent();
        GetCulture();
        MainPage = new NavigationPage(new MainPage());
        HandlePreferencesKeys();
    }

#pragma warning disable CA1822
    void GetCulture()
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (lang == "ar" || lang == "en" || lang == "fr")
            AppValues.Language = lang;
        else
            AppValues.Language = "en";
    }

    void HandlePreferencesKeys()
    {
        try
        {
            DataAccess.Helper.Strings.Port = AppPreferences.Port;
            DataAccess.Helper.Strings.IP = AppPreferences.IP;
            AppValues.LocalBaseURI =  AppPreferences.LocalBaseURI;
            AppValues.LeftScanIcon = true;
        }
        catch
        {
        }
    }
#pragma warning restore CA1822

}
