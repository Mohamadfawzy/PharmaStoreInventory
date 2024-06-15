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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        //await OnboardingText1.TranslateTo(-Width, 0, easing: Easing.SinInOut);

        //OnboardingText2.TranslationX = Width;
        //OnboardingText2.IsVisible = true;
        //new Animation
        //{
        //    { 0, 1.0, new Animation (v => OnboardingText1.TranslationX = v,0, -Width) },
        //    { 0, 1.0, new Animation (v => OnboardingText2.TranslationX = v, Width, 0)}
        //}.Commit(this, "ChildAnimations", 16, 500, easing: Easing.SinInOut);

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
        // OnboardingTextCV.Loop = true;
        if (OnboardingTextCV.Position == 0)
        {
            OnBordimage.Source = "onbord2.jpg";
        }
        else
        {
            OnBordimage.Source = "onbord1.jpg";
        }
    }

    private void GoToLogin(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginView());
    }

    private void GoToRegister(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RegisterView());
    }
}