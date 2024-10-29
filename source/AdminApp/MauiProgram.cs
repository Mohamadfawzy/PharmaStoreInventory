using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace AdminApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("materialdesignicons.ttf", "IconFontMaterial");
                    fonts.AddFont("NotoKufiArabic-ExtraLight.ttf", "KufiExtraLight");
                    fonts.AddFont("NotoKufiArabic-Regular.ttf", "KufiRegular");
                    fonts.AddFont("NotoKufiArabic-SemiBold.ttf", "KufiSemiBold");
                });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
