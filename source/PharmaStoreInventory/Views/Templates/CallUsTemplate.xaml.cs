using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views.Templates;

public partial class CallUsTemplate : ContentView
{
	public CallUsTemplate()
	{
		InitializeComponent();
        labelAppVersionCode.Text = $"الإصدار: {AppConstants.AppVersionCode}";

    }

    private void PhoneDialerTapped(object sender, TappedEventArgs e)
    {
        PhoneDialerService.DialPhoneNumber(AppConstants.ModernSoftPhoneNumber);
    }
}