using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class SettingView : ContentPage
{
	public SettingView()
	{
		InitializeComponent();
		styleSwitch.IsToggled = AppValues.LeftScanIcon;
		quantitySwitch.IsToggled = AppValues.ProductHasQuantityOnly;

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
	
	private void QuantitySwitch_Toggled(object sender, ToggledEventArgs e)
    {
		if (e.Value)
		{
			AppPreferences.ProductHasQuantityOnly = AppValues.ProductHasQuantityOnly = true;
        }
		else
			AppPreferences.ProductHasQuantityOnly = AppValues.ProductHasQuantityOnly = false;
    }
}