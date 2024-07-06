using CommunityToolkit.Maui.Views;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    private PopupWin popupWin;
    public UserView()
    {
        InitializeComponent();
        popupWin = new PopupWin();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // o 1  النفيجيشن ظهر فعلا
    }


    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // o2  اتحملت فعلا
        //var inputFaild = new AnimatedInput();


    }


    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // o 3  اخر حاجة
        //refresh.IsRefreshing = true;

        if (container.Count == 0)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //var inputFaild1 = new AnimatedInput();
            //inputFaild1.InputPlaceholder = "حقل جديد";
            //container.Add(inputFaild1);
            //}
        }

    }


    // ========================== c ======================================
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // c 1

    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        // c 2
    }
    private void ContentPage_NavigatingFrom(object sender, NavigatingFromEventArgs e)
    {
        // c 3 النافيجيشن بتاع الجديدة ظهر
    }

    private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
    {
        // c 4
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        // c 5
        //container.Clear();
        popupWin?.Close();
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.VerificationView());
        popupWin ??= new PopupWin();
        this.ShowPopup(popupWin);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        saveButton.IsVisible = true;
        editIcon.IsVisible = false;
        nameStack.BackgroundColor = Color.FromArgb("#f0f7ff");
        nameEntry.IsReadOnly = false;
        nameEntry.Focus();

    }

    private void saveButton_Clicked(object sender, EventArgs e)
    {
        saveButton.IsVisible = false;
        editIcon.IsVisible = true;
        nameStack.BackgroundColor = Colors.Transparent;
        nameEntry.Unfocus();
        nameEntry.IsReadOnly = true;
    }

    private void MenuItemTapped(object sender, TappedEventArgs e)
    {

    }

    private void ClosePopupClicked(object sender, EventArgs e)
    {
        ClosePopup();

    }

    private void LogoutTapped(object sender, TappedEventArgs e)
    {

    }

    private void ResetPasswordTapped(object sender, TappedEventArgs e)
    {
        OpenPopup();
    }

    void ClosePopup()
    {
        backgroundTransparence.IsVisible = false;
        
        popup.IsVisible = false;
        oldPassword.ClearText();
        password.ClearText();
        confirmPassword.ClearText();
    }

    void OpenPopup()
    {
        backgroundTransparence.IsVisible = true;
        popup.IsVisible = true;
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        oldPassword.HideKeyBoard();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}