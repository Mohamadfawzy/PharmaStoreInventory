namespace DataAccess.Helper;

public class Common
{
    public static string GenerateVerificationCode()
    {
        return new Random().Next(1000, 9999).ToString();
    }
}
