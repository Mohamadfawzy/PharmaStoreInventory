using Microsoft.Maui.Controls.Shapes;
using PharmaStoreInventory.Models;

namespace PharmaStoreInventory.Views.Templates;

public partial class NotificationTemplate : ContentView
{
    Guid token;
    bool isRunning = false;
    bool statusBarChange = false;
    TimeSpan Duration = TimeSpan.FromSeconds(5);

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
        Hiding();
    }
    //CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.Parse("#b9ddfe"));
    //CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);



    public async void ShowMessage2(ErrorMessage model)
    {
        try
        {
            if (isRunning)
            {
                await HideMessage4();
            }
            isRunning = true;
            token = Guid.NewGuid();
            this.IsVisible = true;
            Message = model.Title;
            bodyMessage.Text = model.Body;
            container.Opacity = 1;
            await container.TranslateTo(0, 0);
            container.TranslationY = 0;
            HideMessage2(token);

        }
        catch
        {
            this.IsVisible = true;
            Message = model.Title;
            bodyMessage.Text = model.Body;
            container.Opacity = 1;
            container.TranslationY = 0;
            isRunning = false;
        }
    }


    public async void HideMessage2(Guid theToken)
    {
        await Task.Delay(Duration);
        if (theToken != token)
            return;

        var t1 = container.TranslateTo(0, -container.Height);
        var t2 = container.FadeTo(1);

        await Task.WhenAll(t1, t2);
        this.IsVisible = false;
        isRunning = false;
    }

    public async Task HideMessage4()
    {
        var t1 = container.TranslateTo(0, -container.Height);
        var t2 = container.FadeTo(1);
        await Task.WhenAll(t1, t2);
    }

    private void HideMessage3()
    {
        container.Opacity = 0;
        this.IsVisible = false;
        container.TranslationY = -container.Height; // إخفاء الإشعار خارج الشاشة.
    }


    public void Display(string title)
    {
        Show(title, "");
    }

    public void Display(string title, string body)
    {
        Show(title, body);
    }

    public void Display(ErrorMessage model)
    {
        Show(model.Title, model.Body);
    }

    public void Display(ErrorMessage model, bool statusBar)
    {

        container.StrokeShape = new RoundRectangle
        {
            CornerRadius = new CornerRadius(0)
        };
        container.Margin = 0;
        Show(model.Title, model.Body, statusBar);

    }

    private async void Show(string title, string body, bool statusBar = false)
    {
        try
        {
            if (isRunning)
            {
                await PopAnimate();
            }
            if (statusBar)
            {
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.Parse("#7bc2fe"));
                statusBarChange = true;
            }
            isRunning = true;
            token = Guid.NewGuid();
            this.IsVisible = true;
            Message = title;
            bodyMessage.Text = body;
            container.Opacity = 1;
            await container.TranslateTo(0, 0);
            container.TranslationY = 0;
            Hiding(token);

        }
        catch
        {
            this.IsVisible = true;
            Message = title;
            bodyMessage.Text = body;
            container.Opacity = 1;
            container.TranslationY = 0;
            isRunning = false;
        }
    }

    private async void Hiding()
    {
        await PopAnimate();
        isRunning = false;

    }

    private async void Hiding(Guid theToken)
    {
        await Task.Delay(Duration);
        if (theToken != token)
            return;

        await PopAnimate();
        isRunning = false;
    }


    private async Task PopAnimate()
    {
        try
        {
            var t1 = container.TranslateTo(0, -container.Height);
            var t2 = container.FadeTo(1);
            await Task.WhenAll(t1, t2);
            this.IsVisible = false;
            RestStatusBar();
        }
        catch
        {
            SetAsDefault();
        }
    }

    void SetAsDefault()
    {
        this.IsVisible = false;
        container.Opacity = 0;
        container.TranslationY = 0;
        isRunning = false;
        RestStatusBar();
    }
    void RestStatusBar()
    {
        if (statusBarChange)
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.Parse("#054887"));
    }
}