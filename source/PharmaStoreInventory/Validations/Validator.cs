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
        Regex regex = new(pattern);
        return regex.IsMatch(email);
    }

    public static bool IsValidTelephone(string phoneNumber)
    {
        // Define a regular expression pattern for phone numbers
        var phoneNumberPattern = @"^(\+?\d{1,4}[\s-]?)?(\(?\d{3}\)?[\s-]?)\d{3}[\s-]?\d{4}$";
        return Regex.IsMatch(phoneNumber, phoneNumberPattern);

    }

    internal static bool IsNetworkAccess()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;

        if (accessType != NetworkAccess.Internet)
        {
            return true;
        }
        return false;
    }

}
