namespace PharmaStoreInventory.Views.Templates;

public partial class CLInPickingTemplate : ContentView
{
	public CLInPickingTemplate()
	{
		InitializeComponent();
	}

    private void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (collection.SelectedItem == null) return;
        if (collection.SelectedItem != null)
        {
            //popup.IsVisible = true;
            //backgroundTransparence.IsVisible = true;
        }
        collection.SelectedItem = null;
    }
    private void OnDoneSwipeItemInvoked(object sender, EventArgs e)
    {
        //DisplayAlert("title", "massage", "cancel");
    }

    private async void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {

        if (e.SwipeDirection == SwipeDirection.Right)
        {
            var swip = sender as SwipeView;
            //var first = swip?.LeftItems.First() as SwipeItemView;
            if (swip?.FindByName("checkIcon") is Label checkIcon)
            {
                await checkIcon.TranslateTo(0, 10);
                await checkIcon.ScaleTo(1.2, 400);
                await checkIcon.ScaleTo(0.8, 400);
                await checkIcon.ScaleTo(1, 400);
                await checkIcon.TranslateTo(0, 0);
            }

        }
    }
}