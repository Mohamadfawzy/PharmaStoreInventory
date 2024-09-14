using CommunityToolkit.Maui.Core;
using PharmaStoreInventory.Models;

namespace PharmaStoreInventory.Views.Templates;

public partial class NotificationTemplate : ContentView
{
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(
            nameof(Message),
            typeof(string),
            typeof(NotificationTemplate),
            "", BindingMode.OneWay);

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    public NotificationTemplate()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        HideMessage();
    }

    public async void ShowMessage(ErrorMessage model)
    {
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.Parse("#b9ddfe"));
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
        this.IsVisible = true;
        Message = model.Title;
        bodyMessage.Text = model.Body;
        container.Opacity = 1;
        await container.TranslateTo(0,0);
        container.TranslationY = 0;

        await Task.Delay(TimeSpan.FromSeconds(15));
        HideMessage();
    }
    
    public async void ShowMessage(string title, string body= "")
    {
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.Parse("#b9ddfe"));
        //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
        this.IsVisible = true;
        Message = title;
        bodyMessage.Text = body;
        container.Opacity = 1;
        await container.TranslateTo(0,0);
        container.TranslationY = 0;

        await Task.Delay(TimeSpan.FromSeconds(15));
        HideMessage();
    }


    public async void HideMessage()
    {
        var t1 = container.TranslateTo(0, -80);
        var t2 = container.FadeTo(1);

        await Task.WhenAll(t1, t2);
        this.IsVisible = false;
    }
}