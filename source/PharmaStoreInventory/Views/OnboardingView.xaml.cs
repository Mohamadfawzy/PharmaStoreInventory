using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class OnboardingView : ContentPage
{
    short position = 1;
    #region OnStart
    public OnboardingView()
    {
        InitializeComponent();
        AppPreferences.LocalDeviceId = Guid.NewGuid().ToString();
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await Permissions.RequestAsync<Permissions.Camera>();
    }
    #endregion

    #region OnClicked
    private void GoToLogin(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginView());
    }

    private void GoToRegister(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterView());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            switch (position)
            {
                case 1:
                    await ChangeIndicatorBoxColor(indicatorBox1, indicatorBox2);
                    await Move(bord1, bord2);
                    AppPreferences.IsFirstTime = false;
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
    #endregion

    #region On process
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
        if (Application.Current != null)
        {
            currentBox.Color = (Color)Application.Current.Resources["Primary700"];
            nextBox.Color = (Color)Application.Current.Resources["White"];
        }
        return Task.CompletedTask;
    }
    #endregion
}