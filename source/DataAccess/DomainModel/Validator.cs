using System.Text.RegularExpressions;

namespace DataAccess.DomainModel;

public class Validator
{
    public static  bool IsNumeric(string searchText)
    {
        string pattern = @"\p{Sc}+\s*\d+";
        return Regex.IsMatch(searchText, pattern);
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        Regex regex = new(pattern);
        return regex.IsMatch(email);
    }

    public static bool IsValidTelephone(string phoneNumber)
    {
        // Define a regular expression pattern for phone numbers
        var phoneNumberPattern = @"^(\+?\d{1,4}[\s-]?)?(\(?\d{3}\)?[\s-]?)\d{3}[\s-]?\d{4}$";

        // Check if the text matches the pattern
        return Regex.IsMatch(phoneNumber, phoneNumberPattern);
    }
}
