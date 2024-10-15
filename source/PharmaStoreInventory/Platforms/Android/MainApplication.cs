using Android.App;
using Android.Content.Res;
using Android.Runtime;

namespace PharmaStoreInventory
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
                if (view is Entry)
                {
                    // RemoveById underline
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    //handler.PlatformView?.TextCursorDrawable?.SetTint(Colors.Red.ToPlatform());
                    // Change placeholder text color
                    // handler.PlatformView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Red));
                }
            });


            Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping(nameof(Label), (handler, view) =>
            {
                try
                {
                    if (view is Label label)
                    {
                        var description = SemanticProperties.GetDescription(label);
                        if (description == "Justify")
                        {
                            // Apply text justification on Android
                            handler.PlatformView.JustificationMode = Android.Text.JustificationMode.InterWord;
                        }
                        else
                        {
                            // Reset or apply default alignment for non-justified labels
                            handler.PlatformView.JustificationMode = Android.Text.JustificationMode.None;
                        }
                    }
                }
                catch 
                {
                }
            });
        }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
