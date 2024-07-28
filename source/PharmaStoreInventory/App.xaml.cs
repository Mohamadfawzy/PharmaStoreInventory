using DataAccess.Contexts;
using Microsoft.Maui.Storage;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Views;
using System.Globalization;

namespace PharmaStoreInventory;
public partial class App : Application
{
    public static AppDb Context = null!;
    public App()
    {
        //Context = new AppDb();
        InitializeComponent();
        GetCulture();
        MainPage = new NavigationPage(new MainPage());
        //GetPreferencesKeys();
    }

    void GetCulture()
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (lang == "ar" || lang == "en" || lang == "fr")
            AppConstants.Language = lang;
        else
            AppConstants.Language = "en";
    }

    void GetPreferencesKeys()
    {
        try
        {
            bool hasKey = Preferences.Default.ContainsKey("Port");
            if (hasKey)
            {
                DataAccess.Helper.Constants.Port = Preferences.Default.Get("Port", "1433");
                DataAccess.Helper.Constants.IP = Preferences.Default.Get("IP", "192.168.1.103");
            }
        }
        catch (Exception)
        {
        }
    }
}
