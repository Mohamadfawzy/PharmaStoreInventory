namespace PharmaStoreInventory.Services;

internal static class PhoneDialerService
{
    public  static void DialPhoneNumber(string phoneNumber)
    {
        if (PhoneDialer.Default.IsSupported)
            PhoneDialer.Default.Open(phoneNumber);
    }
}
