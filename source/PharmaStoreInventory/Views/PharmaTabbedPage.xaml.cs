namespace PharmaStoreInventory.Views;

public partial class PharmaTabbedPage : TabbedPage
{
	public PharmaTabbedPage()
	{
		InitializeComponent();
	}

    public void GoToAllStockView()
    {
        // Assuming "All Stock" is the third tab
        this.CurrentPage = this.Children[2];
    }

    public void GoToRegisterView()
    {
        // Assuming "Register" is the first tab
        this.CurrentPage = this.Children[0];
    }

    public void GoToWaitingApprovalView()
    {
        // Assuming "Waiting Approval" is the second tab
        this.CurrentPage = this.Children[1];
    }

}