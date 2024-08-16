using PharmaStoreInventory.Views;

namespace PharmaStoreInventory
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AllStockView), typeof(AllStockView));
            Routing.RegisterRoute(nameof(PickingView), typeof(PickingView));
            Routing.RegisterRoute(nameof(RegisterView), typeof(RegisterView));
        }
    }
}
