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

    void GetCulture()
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (lang == "ar" || lang == "en" || lang == "fr")
            AppConstants.Language = lang;
        else
            AppConstants.Language = "en";
    }

    void HandlePreferencesKeys()
    {
        try
        {
            DataAccess.Helper.Constants.Port = AppPreferences.Port;
            DataAccess.Helper.Constants.IP = AppPreferences.IP;
        }
        catch
        {
        }
    }
}
