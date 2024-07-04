namespace PharmaStoreInventory.Helpers;

internal class IconFontManager
{
    private const string ArrowRight = "\U000f0054";
    private const string ArrowLeft = "\U000f004d";



    internal static string ArrowIcon
    {
        get
        {
            if (AppConstants.Language == "ar")
                return ArrowRight;
            else
                return ArrowLeft;
        }
    }
}
