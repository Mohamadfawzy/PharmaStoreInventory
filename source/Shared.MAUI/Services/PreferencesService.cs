namespace Shared.MAUI.Services;

public class PreferencesService
{
    public static void SetPreference(string key, string value)
    {
        Preferences.Set(key, value);
    }

    public static T GetPreference<T>(string key, T defaultValue)
    {
        if (Preferences.Default.ContainsKey(key))
        {
            return Preferences.Default.Get(key, defaultValue);
        }
        return defaultValue;
    }
    public static void RemovePreference(string key)
    {
        Preferences.Remove(key);
    }

    public static void ClearPreferences()
    {
        Preferences.Clear();
    }
}
 