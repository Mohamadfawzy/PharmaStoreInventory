using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Views;
using System.Globalization;

namespace PharmaStoreInventory;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        GetCulture();
        MainPage = new  NavigationPage(new MainPage());
    }

    void GetCulture()
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        if (lang == "ar" || lang == "en" || lang == "fr")
            AppConstants.Language =  lang;
        else
            AppConstants.Language = "en";
    }
}
