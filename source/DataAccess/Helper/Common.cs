namespace DataAccess.Helper;

public class Common
{
    public static string GenerateVerificationCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}
