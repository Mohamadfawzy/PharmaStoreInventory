using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views;

public partial class OnbordingView : ContentPage
{
    List<string> OnboardingTextList = new List<string>()
    {
        Languages.AppResources.onbording_OnboardingText1,
        "للبدء، قم بإدخال بيانات تسجيل الدخول الخاصة بك. تأكد من صحة المعلومات لضمان الوصول السريع إلى بيانات صيدليتك.يمكنك إضافة وتحديث الأصناف والأدوية بسهولة باستخدام ماسح الباركود أو البحث اليدوي. استعرض تقارير دقيقة وشاملة لتحليل أداء المخزون والمبيعات، واتخذ قرارات مستنيرة للحفاظ على صيدليتك في أفضل حالاتها."


    };
    public OnbordingView()
    {
        InitializeComponent();
        OnboardingTextCV.ItemsSource = OnboardingTextList;
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
}