using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccess.Helper;

public static class Constants
{
    public static string? IP = string.Empty; //= "192.168.1.103";
    public static string? Port = string.Empty; //= 1433;

    
    public const string UserId = "1001";
    public const int VerificationCodeMinutesExpires = 30;

}
