namespace PharmaStoreInventory.Helpers;
internal static class AppValues
{
    public static string UserFileName = FileSystem.AppDataDirectory + "/user.json";
    public static string BranchsFileName = FileSystem.AppDataDirectory + "/branchs.json";
    public static string XBranchsFileName = FileSystem.AppDataDirectory + "/XBarchs.xml";
    public static string Language { get; set; } = "ar";
    //public static string VerificationCode { get; set; } = string.Empty;
    //public static string VerificationCodeFromUser { get; set; } = null!;
    //public static string VerificationCodeSended { get; set; } = null!;
    //public static string ConnectionString = "Server=192.168.1.103,1433;Database=stock;User Id=sa;Password=Ph@store;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True";

    public static string HostBaseURI = "http://192.168.1.20:5219/api";
    //public static string HostBaseURI = "http://estock.somee.com/api";
    public static string LocalBaseURI = "http://000.000.000.000:5144/api";
    public static bool LeftScanIcon = false;
    public static bool ProductHasQuantityOnly = false;
}
