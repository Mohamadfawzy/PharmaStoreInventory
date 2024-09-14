using CommunityToolkit.Mvvm.Messaging;
using DrasatHealthMobile.Helpers;
using PharmaStoreInventory.Messages;

namespace PharmaStoreInventory.Views;

public partial class DashboardView : ContentPage, IRecipient<DashboardViewNotification>
{
    public DashboardView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<DashboardViewNotification>(this);

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }


    private async Task EdgeAlert(string text, string title = "Alarm")
    {

        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Colors.Red);
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);


        var alertImage = new Label { Text = IconFont.BellRing };
        var stack = new StackLayout
        {
            BackgroundColor = Colors.White,
            Children =
                {
                    alertImage,
                    new StackLayout()
                    {
                        Padding =new Thickness (0,10,0,0),
                        Children =
                        {
                            new Label { Text = title,FontSize=22 },
                            new Label { Text = text },

                        }

                    }
                }
        };
        MainGrid.Children.Add(stack);
        MainGrid.SetRowSpan(stack, 5);
        var an = new Animation();
        var taskCompletionSource = new TaskCompletionSource<bool>();

        an.Add(0, 0.1, new Animation(v => stack.TranslationY = v, -80, 0));

        an.Add(0.1, 0.2, new Animation(v => alertImage.Rotation = v, 0, 5));
        an.Add(0.2, 0.3, new Animation(v => alertImage.Rotation = v, 5, 0));
        an.Add(0.3, 0.4, new Animation(v => alertImage.Rotation = v, 0, -5));
        an.Add(0.4, 0.5, new Animation(v => alertImage.Rotation = v, -5, 0));
        an.Add(0.5, 0.6, new Animation(v => alertImage.Rotation = v, 0, 5));
        an.Add(0.6, 0.7, new Animation(v => alertImage.Rotation = v, 5, 0));

        an.Add(0.9, 1, new Animation(v => stack.TranslationY = v, 0, -81));
        an.Commit(owner: stack, "alert", length: 700 + 2000 + 300, easing: Easing.SinInOut,
         finished: (x, c) =>
         {
             taskCompletionSource.SetResult(c);
             MainGrid.Children.Remove(stack);
             CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Colors.Green);

         });
    }



    private async void HamburgerTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new UserView());
        notification.ShowMessage(new Models.ErrorMessage("Title","Body"));
        //var v = (BindingContext as ViewModels.DashboardViewModel);
        //v.IsPlaceholderElementVisible = !v.IsPlaceholderElementVisible;
    }

    private async void GoToScanPage(object sender, TappedEventArgs e)
    {
        //Navigation.PushAsync(new Views.BarcodeReaderView());
        //await Navigation.PushAsync(new Views.AllStockView());
        await Navigation.PushAsync(new Views.PickingView());
    }

    //private void OpenPopupTapped(object sender, TappedEventArgs e)
    //{
    //    OpenPopup();
    //}

    //private void ClosePopupTapped(object sender, TappedEventArgs e)
    //{
    //    ClosePopup();
    //}

    //void ClosePopup()
    //{
    //    popup.IsVisible = false;
    //}

    //void OpenPopup()
    //{
    //    popup.IsVisible = true;
    //}

    private async void GoToAllProductsPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AllStockView());
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {

    }

    public void Receive(DashboardViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.ShowMessage(message.Value);
        });
    }

    private async void placeholderElement_Loaded(object sender, EventArgs e)
    {
        //while (placeholderElement.IsVisible)
        //{
        //    placeholderElement.Opacity = 0.7;
        //    await Task.Delay(TimeSpan.FromSeconds(2));
        //    placeholderElement.Opacity = 1;
        //    await Task.Delay(TimeSpan.FromSeconds(2));
        //}

        //if (!placeholderElement.IsVisible)
        //{
        //    placeholderElement.Opacity = 1;
        //}
    }

    private async void BoxView_Loaded(object sender, EventArgs e)
    {
        var box = (BoxView)sender;

        while (placeholderElement.IsVisible)
        {
            await box.ScaleTo(0.98).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            await box.ScaleTo(1).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
        }

        if (!placeholderElement.IsVisible)
        {
            box.Scale = 1;
        }
    }
}