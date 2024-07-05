using CommunityToolkit.Mvvm.ComponentModel;
using PharmaStoreInventory.Models;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel: ObservableObject
{
    public List<StockModel> StockModelListTemp;
    public List<StockModel> StockModelList { get; set; }
    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public AllStockViewModel()
    {
        StockModelList = new();
        StockModelListTemp = new();
        GetStockModelList();
    }
    private async void SearchBoxTyping(string text)
    {
        await GetFromSearch(text);
    }
    private async Task GetFromSearch(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text))
            {
                StockModelList = StockModelListTemp;
                OnPropertyChanged(nameof(StockModelList));
                return;
            }

            StockModelList = Services.MockData.GetStocksByText(text.ToLower());
            
            OnPropertyChanged(nameof(StockModelList));
        }
        catch (Exception ex)
        {
            Helpers.CatchingException.PharmaDisplayAlert(ex.Message) ;
        }
    }
    void GetStockModelList()
    {
        StockModelList = StockModelListTemp= Services.MockData.GetStocksNonRepet();
    }
}
