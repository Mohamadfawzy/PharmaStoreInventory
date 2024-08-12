using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataAccess.Helper;

public static class Strings
{
    public static string IP { get; set; } = string.Empty;
    public static string Port {  get; set; } = string.Empty;

    public static string UserId { get; set; } = "1001";

    public static int VerificationCodeMinutesExpires { get; set; }  = 30;

}
