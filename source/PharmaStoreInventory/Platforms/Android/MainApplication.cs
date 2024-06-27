﻿using Android.App;
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
                    // Remove underline
                    handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    //handler.PlatformView?.TextCursorDrawable?.SetTint(Colors.Red.ToPlatform());
                    // Change placeholder text color
                    // handler.PlatformView.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.Red));
                }
            });


        }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
