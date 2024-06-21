namespace PharmaStoreInventory.Helpers;

public static class CatchingException
{
    public static void PharmaDisplayAlert(string message)
    {
        App.Current?.MainPage?.DisplayAlert("Exception",message,"cancel");
    }
}
