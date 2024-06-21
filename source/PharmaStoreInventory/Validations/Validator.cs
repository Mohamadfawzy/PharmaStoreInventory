using System.Text.RegularExpressions;

namespace PharmaStoreInventory.Validations;

internal static class Validator
{
    internal static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        // Define a regular expression pattern for a valid email address
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        // Create a Regex object and use it to match the email address
        Regex regex = new(pattern);
        return regex.IsMatch(email);
    }

    public static bool IsValidTelephone(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;
        // Define a regex pattern for a valid US phone number
        string pattern = @"^[0-9]+$";

        // Create a Regex object and use it to match the phone number
        Regex regex = new(pattern);
        return regex.IsMatch(phoneNumber);
    }
}
