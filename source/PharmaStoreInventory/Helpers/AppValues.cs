namespace PharmaStoreInventory.Helpers;
internal static class AppValues
{

    public static string UserFileName = FileSystem.AppDataDirectory + "/user.json";
    public static string BranchesFileName = FileSystem.AppDataDirectory + "/branchs.json";
    public static string XBranchesFileName = FileSystem.AppDataDirectory + "/XBarchs.xml";

    public static string Language { get; set; } = "ar";
    public static string HostBaseURI = "http://estock.somee.com/api";
    public static string LocalBaseURI = "http://000.000.000.000:5144/api";
    public static string SystemVersion = "system";
    public static string PharmaVersion = "pharma";
    public static bool LeftScanIcon = false;
    public static bool ProductHasQuantityOnly = false;
    public static bool IsDevelopment = false;
}
