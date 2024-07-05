namespace PharmaStoreInventory.Views;

public partial class AllStockView : ContentPage
{
	public AllStockView()
	{
		InitializeComponent();
	}

    protected override bool OnBackButtonPressed()
    {
        if (filterIsOpen)
        {
            _ = CloseFilterFrameAninmation();
            return true;
        }
        return base.OnBackButtonPressed();
    }

    private void AllStockList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Navigation.PushAsync(new PickingView());
        //e.CurrentSelection
    }
    bool filterIsOpen = false;
    private async void OpenFiltrFrame(object sender, TappedEventArgs e)
    {
        try
        {
            filterIsOpen = true;
            borderFilter.TranslationY = 800;
            borderFilter.IsVisible = true;
            await borderFilter.TranslateTo(0, 0, length: 500, easing: Easing.CubicInOut);
        }
        catch (Exception)
        {

        }
    }

    private async void CloseFiltrFrame(object sender, EventArgs e)
    {
        await CloseFilterFrameAninmation();
    }

    private async Task CloseFilterFrameAninmation()
    {
        try
        {
            filterIsOpen = false;
            await borderFilter.TranslateTo(0, 800, length: 500, easing: Easing.CubicInOut);
            borderFilter.IsVisible = false;
        }
        catch (Exception)
        {
        }
    }
}