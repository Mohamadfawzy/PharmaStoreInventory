using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Maui.Controls;

namespace PharmaStoreInventory.Helpers;

public static class AppPreferences
{

    public static int HostUserId
    {
        get => GetValue(nameof(HostUserId), 0);
        set => Preferences.Default.Set(nameof(HostUserId),value);
    }

    public static int LocalDbUserId
    {
        get => GetValue(nameof(LocalDbUserId), 0);
        set => Preferences.Default.Set(nameof(LocalDbUserId),value);
    }
    
    public static int StoreId
    {
        get => GetValue(nameof(StoreId), 1);
        set => Preferences.Default.Set(nameof(StoreId),value);
    }
    
    public static bool IsLoggedIn
    {
        get => GetValue(nameof(IsLoggedIn), false);
        set => Preferences.Default.Set(nameof(IsLoggedIn), value);
    }
    
    public static bool HasBranchRegistered
    {
        get => GetValue(nameof(HasBranchRegistered), false);
        set => Preferences.Default.Set(nameof(HasBranchRegistered), value);
    }

    public static string Port
    {
        get => GetValue(nameof(Port), "1433");
        set => Preferences.Default.Set(nameof(Port), value);
    }

    public static string IP
    {
        get => GetValue(nameof(IP), ".");
        set => Preferences.Default.Set(nameof(IP), value);
    }

    public static string GetDeviceID()
    {
        try
        {
            bool hasKey = Preferences.Default.ContainsKey("DeviceId");
            if (hasKey)
            {
                return Preferences.Default.Get<string>("DeviceId", string.Empty);
            }
            else return string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static void SetDeviceID()
    {
        try
        {
            string? deviceID = null;
#if ANDROID
            deviceID = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
#endif
            if (deviceID != null)
            {
                Preferences.Default.Set("DeviceId", deviceID);
            }
            else
            {
                Preferences.Default.Set("DeviceId", Guid.NewGuid().ToString());
            }
        }
        catch
        {
        }
    }

    public static R GetValue<R>(string key, R defaultValue)
    {
        if (Preferences.Default.ContainsKey(key))
        {
            return Preferences.Default.Get<R>(key, defaultValue);
        }
        return defaultValue;
    }

}
