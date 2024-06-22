using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Java.Util.Logging;

namespace PharmaStoreInventory
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Window != null)
            {
                Window.DecorView.LayoutDirection = Android.Views.LayoutDirection.Rtl;
            }
        }
    }
}
