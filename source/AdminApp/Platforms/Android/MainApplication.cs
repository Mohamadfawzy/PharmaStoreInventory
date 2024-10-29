using Android.App;
using Android.Content.Res;
using Android.Runtime;

namespace AdminApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
        {
            // Start renderer
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
                {
                    if (view is Entry)
                    {
                        handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    }
                });
            // End renderer
        }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
