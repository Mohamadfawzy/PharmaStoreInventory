using CommunityToolkit.Maui.Views;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Views;

public partial class UserView : ContentPage
{
    private PopupWin popup;
    public UserView()
    {
        InitializeComponent();
        
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

            //}

            var inputFaild1 = new AnimatedInput();
            inputFaild1.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild1);


            var inputFaild2 = new AnimatedInput();
            inputFaild2.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild2);


            var inputFaild3 = new AnimatedInput();
            inputFaild3.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild3);


            var inputFaild4 = new AnimatedInput();
            inputFaild4.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild4);


            var inputFaild5 = new AnimatedInput();
            inputFaild5.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild5);


            var inputFaild6 = new AnimatedInput();
            inputFaild6.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild6);

            var inputFaild7 = new AnimatedInput();
            inputFaild7.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild7);

            var inputFaild8 = new AnimatedInput();
            inputFaild8.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild8);

            var inputFaild9 = new AnimatedInput();
            inputFaild9.InputPlaceholder = "حقل جديد";
            container.Add(inputFaild9);




            var btn = new Button();
            btn.Text = "OK";
            btn.Clicked += Button_Clicked;

            container.Add(btn);
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
        popup?.Close();  
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.VerificationView());
        popup = new PopupWin();
        this.ShowPopup(popup);
    }
}