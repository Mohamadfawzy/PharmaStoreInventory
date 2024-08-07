namespace PharmaStoreInventory.Helpers;

public static class AppConstants
{
    public const string AppName = "PharmaStoreInventory";
    public const string ApiBaseUrl = "https://api.example.com/";
    public const int MaxRetryAttempts = 3;
    public static string BranchsFileName = FileSystem.AppDataDirectory + "/branchs.json";
    public static string Language { get; set; } = string.Empty;
    public static string VerificationCode { get; set; } = string.Empty;
    //public static bool HasBranchRegistered { get; set; } 

}
