using PharmaStoreInventory.Views;

namespace PharmaStoreInventory
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

        }
        
        private void GoToOnbording1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OnbordingView());
        }
        
        private void GoToLogin(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginView());
        }
        
        private void GoToRegister(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterView());
        }
        
        private void GoToWaitingApprovalView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new WaitingApprovalView());
        }
        private void GoToCreateBranchView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateBranchView());
        }
        
        private void GoToAllStockView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllStockView());
            //var parentTabbedPage = this.Parent as PharmaTabbedPage;
            //parentTabbedPage?.GoToAllStockView();
        }
        
        private void GoToBarcodeReaderView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new BarcodeReaderView());
        }
    }
}
