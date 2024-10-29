using Shared.MAUI.Services;
namespace AdminApp.AppData;

internal static class PreferencesData
{
    public static string UserEmail
    {
        get => PreferencesService.GetPreference(nameof(UserEmail), string.Empty);
        set => PreferencesService.SetPreference(nameof(UserEmail), value);
    }


    public static string UserFullName
    {
        get => PreferencesService.GetPreference(nameof(UserFullName), "مرحبا");
        set => Preferences.Default.Set(nameof(UserFullName), value);
    }

    public static int LocalDbUserId
    {
        get => PreferencesService.GetPreference(nameof(LocalDbUserId), 0);
        set => Preferences.Default.Set(nameof(LocalDbUserId), value.ToString());
    }


}
