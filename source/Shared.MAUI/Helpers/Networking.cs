namespace Shared.MAUI.Helpers;

public class Networking
{
    public static bool IsNetworkAccess()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType != NetworkAccess.Internet)
        {
            //notification.ShowMessage(new Models.ErrorMessage("No NetworkAccess", "please check your WiFi"));
            return true;
        }
        return false;
    }
}
