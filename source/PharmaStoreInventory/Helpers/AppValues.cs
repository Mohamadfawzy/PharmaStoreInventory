namespace PharmaStoreInventory.Helpers;
internal static class AppValues
{

    public static string BranchsFileName = FileSystem.AppDataDirectory + "/branchs.json";
    public static string Language { get; set; } = string.Empty;
    public static string VerificationCode { get; set; } = string.Empty;
    public static string Barcode { get; set; } = null!;
}
