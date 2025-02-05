using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;

namespace PharmaStoreInventory
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;
            if (Window != null)
            {
                //var ss = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                //if (ss == "ar")
                //{
                //    Window.DecorView.LayoutDirection = Android.Views.LayoutDirection.Rtl;
                //    AppValues.Language = "ar";
                //}
                //else
                //{
                //    Window.DecorView.LayoutDirection = Android.Views.LayoutDirection.Ltr;
                //    AppValues.Language = "en";
                //}
                Window.DecorView.LayoutDirection = Android.Views.LayoutDirection.Rtl;
            }

        }
    }
}
