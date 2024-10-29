using AdminApp.AppData;
using AdminApp.Views;

namespace AdminApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            IsDevelopmentMode();
            MainPage = new NavigationPage(new MainPage());
            MainPage.FlowDirection = FlowDirection.RightToLeft;
        }
        private void IsDevelopmentMode()
        {
#if DEBUG
            StaticData.BaseUrl = "http://192.168.1.105:5219/api";
            StaticData.IsDevelopment = true;
#endif
        }
    }
}
