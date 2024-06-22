using PharmaStoreInventory.Views;

namespace PharmaStoreInventory;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new  NavigationPage(new OnbordingView());
    }
}
