using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class SettingView : ContentPage
{
	public SettingView()
	{
		InitializeComponent();
		styleSwitch.IsToggled = AppPreferences.LeftScanIcon;

    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
		if (e.Value)
		{
			AppPreferences.LeftScanIcon = AppValues.LeftScanIcon = true;
        }
		else
			AppPreferences.LeftScanIcon = AppValues.LeftScanIcon = false;
    }
}