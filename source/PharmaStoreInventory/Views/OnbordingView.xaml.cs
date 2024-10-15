using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class OnbordingView : ContentPage
{
    List<string> OnboardingTextList = new List<string>()
    {
        Languages.AppResources.onbording_OnboardingText1,
    };
    public OnbordingView()
    {
        InitializeComponent();
        OnboardingTextCV.ItemsSource = OnboardingTextList;
        AppPreferences.LocalDeviceId = Guid.NewGuid().ToString();
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Permissions.RequestAsync<Permissions.Camera>();
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (OnboardingTextCV.Position == 0)
        {
            OnboardingTextCV.Position = 1;
        }
        else if (OnboardingTextCV.Position == 1)
        {
            OnboardingTextCV.IsVisible = false;
            indicatorView.IsVisible = false;
            ArrowNext.IsVisible = false;
            LoginActionsLayout.IsVisible = true;
        }
    }

    private void OnboardingTextCV_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        OnboardingTextCV.Loop = true;
        if (OnboardingTextCV.Position == 0)
        {
            OnBordimage.Source = "pharma2.jpg";
        }
        else
        {
            OnBordimage.Source = "pharma1.jpg";
            AppPreferences.IsFirstTime = false;
        }
    }

    private void GoToLogin(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginView());
        //App.Current.MainPage = new NavigationPage(new PharmaTabbedPage());
        //Navigation.NavigationStack.LastOrDefault()

    }

    private void GoToRegister(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterView());
    }
    short position = 1;
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            switch (position)
            {
                case 1:
                    await ChangeIndicatorBoxColor(indicatorBox1, indicatorBox2);
                    await Move(bord1, bord2);
                    position = 2;
                    break;

                case 2:
                    await ChangeIndicatorBoxColor(indicatorBox2, indicatorBox1);
                    await Move(bord2, bord1);
                    position = 1;
                    break;
            }
        }
        catch
        {
        }
    }

    private async Task Move(View currentView, View nextView)
    {
        nextView.IsVisible = true;
        var t1 = currentView.FadeTo(0);
        var t2 = nextView.FadeTo(1);
        currentView.IsVisible = false;
        await Task.WhenAll(t1, t2);
    }
    
    
    private Task ChangeIndicatorBoxColor(BoxView currentBox, BoxView nextBox)
    {
        if(Application.Current!= null)
        {
            currentBox.Color = (Color)Application.Current.Resources["Primary700"];
            nextBox.Color = (Color)Application.Current.Resources["White"];
        }
        return Task.CompletedTask;
    }
}