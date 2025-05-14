namespace PharmaStoreInventory.Helpers;

public static class AppPreferences
{

    public static int HostUserId
    {
        get => GetValue(nameof(HostUserId), 0);
        set => Preferences.Default.Set(nameof(HostUserId),value);
    }

    public static string UserFullName
    {
        get => GetValue(nameof(UserFullName), "بك");
        set => Preferences.Default.Set(nameof(UserFullName), value);
    }

    public static string Token
    {
        get => GetValue(nameof(Token), "Token");
        set => Preferences.Default.Set(nameof(Token),value);
    }
    
    public static string UserEmail
    {
        get => GetValue(nameof(UserEmail), string.Empty);
        set => Preferences.Default.Set(nameof(UserEmail), value);
    }
    
    public static string UserPassword
    {
        get => GetValue(nameof(UserPassword), string.Empty);
        set => Preferences.Default.Set(nameof(UserPassword), value);
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
    
    public static int PageSize
    {
        get => GetValue(nameof(PageSize), 20);
        set => Preferences.Default.Set(nameof(PageSize),value);
    }
    
    public static bool IsLoggedIn
    {
        get => GetValue(nameof(IsLoggedIn), false);
        set => Preferences.Default.Set(nameof(IsLoggedIn), value);
    }

    public static bool IsFirstTime
    {
        get => GetValue(nameof(IsFirstTime), true);
        set => Preferences.Default.Set(nameof(IsFirstTime), value);
    }
    
    public static bool IsUserActivated
    {
        get => GetValue(nameof(IsUserActivated), false);
        set => Preferences.Default.Set(nameof(IsUserActivated), value);
    }
    
    public static bool HasBranchRegistered
    {
        get => GetValue(nameof(HasBranchRegistered), false);
        set => Preferences.Default.Set(nameof(HasBranchRegistered), value);
    }
    
    public static bool LeftScanIcon
    {
        get => GetValue(nameof(LeftScanIcon), false);
        set => Preferences.Default.Set(nameof(LeftScanIcon), value);
    }
    
    public static bool ProductHasQuantityOnly
    {
        get => GetValue(nameof(ProductHasQuantityOnly), false);
        set => Preferences.Default.Set(nameof(ProductHasQuantityOnly), value);
    }

    public static string LocalBaseURI
    {
        get => GetValue(nameof(LocalBaseURI), "http://192.168.1.100:5144/api");
        set => Preferences.Default.Set(nameof(LocalBaseURI), value);
    }

    public static string LocalDeviceId
    {
        get => GetValue("LocalDeviceId", ".");
        set => Preferences.Default.Set("LocalDeviceId", value);
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
                Preferences.Default.Set("DeviceId", LocalDeviceId);
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
