namespace PharmaStoreInventory.Helpers;
internal static class AppValues
{
    public static string BranchsFileName = FileSystem.AppDataDirectory + "/branchs.json";
    public static string Language { get; set; } = string.Empty;
    public static string VerificationCode { get; set; } = string.Empty;
    public static string NavigationProductCode { get; set; } = null!;
    public static string VerificationCodeFromUser { get; set; } = null!;
    public static string VerificationCodeSended { get; set; } = null!;
}
