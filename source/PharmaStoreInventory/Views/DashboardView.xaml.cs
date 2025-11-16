using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace PharmaStoreInventory.Views;

public partial class DashboardView : ContentPage, IRecipient<DashboardViewNotification>
{
    private readonly DashboardViewModel vm;
    private static string mode = "inventory";

    #region OnStart
    public DashboardView()
    {
        InitializeComponent();
        vm = (DashboardViewModel)BindingContext;
        WeakReferenceMessenger.Default.Register<DashboardViewNotification>(this);
        double statusBarHeight = Application.Current?.MainPage?.Padding.Top ?? 0;
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        popup.HeightRequest = this.Height;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var active = await vm.IsUserActive();
        if (!active)
        {
            Logout();
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            //if (AppPreferences.StartStockId < 1)
            //{
            //    btnLastStartStock.IsVisible = false;
            //}
            //else
            //{
            //    btnLastStartStock.IsVisible = true;
            //}

            if (mode == "sidebar")
            {
                mode = "inventory";
                ResetAllStyles();
                HighlightSelected(imgInventory, lblInventory);
                SwetchBetwwen2Tabps();

            }
        });
        if (IsLatestStartStockDateNotToday())
        {
            AppPreferences.StartStockId = 0;
        }
        ;
            //mode = "inventory";
            //SwetchBetwwen2Tabps();
            //ResetAllStyles();
            //HighlightSelected(imgInventory, lblInventory);
    }
    protected override bool OnBackButtonPressed()
    {
        if (vm.CanBackButtonPressed())
        {
            return base.OnBackButtonPressed();
        }
        return true;
    }

    public void Receive(DashboardViewNotification message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.Display(message.Value);
        });
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var prefix = "مرحباً";
        userFullName.Text = $"{prefix} {AppPreferences.UserFullName}";
    }
    #endregion

    #region OnClicked
    private async void HamburgerTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SidebarView());
    }

    private async void GoToScanPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PickingView());
    }

    private async void GoToAllProductsPage(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AllStockView());
    }

    private async void GoToProductSearchPage_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ProductSearchView());
    }


    //private async void GoToStartStockPage(object sender, TappedEventArgs e)
    //{
    //    await Navigation.PushAsync(new PickingProductsView());
    //}

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
    #endregion

    private void Logout()
    {
        File.Delete(AppValues.XBranchesFileName);
        File.Delete(AppValues.UserFileName);
        File.Delete(AppValues.BranchesFileName);

        AppPreferences.LocalDbUserId = 0;
        AppPreferences.StoreId = 1;
        AppPreferences.HasBranchRegistered = false;
        AppPreferences.LeftScanIcon = false; 

        if (Application.Current != null)
            Application.Current.MainPage = new NavigationPage(new LoginView());
        AppPreferences.IsLoggedIn = false;
        AppPreferences.HostUserId = 0;

        // delete all be low
        AppPreferences.StoreId = 0;
    }

    // =============================================
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string parameter)
        {
            ResetAllStyles();
            mode = parameter;
            SwetchBetwwen2Tabps();
            switch (parameter)
            {
                case "inventory":
                    HighlightSelected(imgInventory, lblInventory);
                    break;

                case "startstock":
                    HighlightSelected(imgStartStock, lblStartStock);
                    break;

                case "sidebar":
                    HighlightSelected(imgMenu, lblMenu);
                    await Navigation.PushAsync(new SidebarView());
                    break;
            }
        }
    }

    private void HighlightSelected(Image image, Label label)
    {
        var behavior = new IconTintColorBehavior
        {
            TintColor = Color.FromArgb("#0a88ed")
        };
        image.Behaviors.Add(behavior);
        label.FontSize = 12;
        label.TextColor = Color.FromArgb("#0a88ed");
    }

    private void ResetAllStyles()
    {
        var images = new[] { imgInventory, imgStartStock, imgMenu };
        var labels = new[] { lblInventory, lblStartStock, lblMenu };

        foreach (var img in images)
        {
            var behavior = new IconTintColorBehavior
            {
                TintColor = Color.FromArgb("#999999")
            };
            img.Behaviors.Add(behavior);
        }

        foreach (var lbl in labels)
        {
            lbl.FontSize = 12;
            lbl.TextColor = Colors.Gray;
        }
    }

    private void SwetchBetwwen2Tabps()
    {
        if(mode == "inventory")
        {
            grdInventoty.IsVisible = true;
            grdStartStock.IsVisible = false;
            //brdLastInverotyDate.IsVisible = true;
        }
        else if (mode == "startstock")
        {

            grdInventoty.IsVisible = false;
            grdStartStock.IsVisible = true;
            //brdLastInverotyDate.IsVisible = false;
        }
    }


    public static bool IsLatestStartStockDateNotToday()
    {
        // إذا لم يكن هناك تاريخ محفوظ
        if (string.IsNullOrWhiteSpace(AppPreferences.LatestStartStockDate))
            return true;

        // محاولة تحويل النص إلى تاريخ
        if (!DateTime.TryParseExact(
                AppPreferences.LatestStartStockDate,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime savedDate))
        {
            // إذا حدث خطأ في التحويل اعتبره ليس تاريخ اليوم
            return true;
        }

        // مقارنة التاريخ المحفوظ بتاريخ اليوم (بدون وقت)
        return savedDate.Date != DateTime.Now.Date;
    }



}