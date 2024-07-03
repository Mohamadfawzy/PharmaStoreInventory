using PharmaStoreInventory.Views;
using System.Windows.Input;

namespace PharmaStoreInventory;

public partial class MainPage : ContentPage
{
    public ICommand NavigateCommand { get; private set; }

    public MainPage()
    {
        InitializeComponent();

        NavigateCommand = new Command<Type?>(
            async (Type? pageType) =>
            {
                if (pageType != null)
                {
                    Page? page = Activator.CreateInstance(pageType) as Page;
                    await Navigation.PushAsync(page);
                }
            });

        BindingContext = this;

    }

    private void GoToStockDetailsView(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StockDetailsView("6221068000977"));
    }

}
