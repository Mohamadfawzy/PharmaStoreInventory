namespace PharmaStoreInventory.Helpers;

internal class IconFontManager
{
    private const string ArrowRight = "\U000f0054";
    private const string ArrowLeft = "\U000f004d";

    private const string ChevronDoubleLeft = "\U000f013d";
    private const string ChevronDoubleRight = "\U000f013e";

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
    
    internal static string ChevronDouble
    {
        get
        {
            if (AppConstants.Language == "ar")
                return ChevronDoubleLeft;
            else
                return ChevronDoubleRight;
        }
    }
}
