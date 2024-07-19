using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Repository;
using PharmaStoreInventory.Models;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel: ObservableObject
{
    private ProductAmountRepo repo;
    private AppDb context;

    // ########*Public*########
    public ProductQueryParameters ProductQueryParam { get; set; }
    public List<StockModel> StockModelListTemp;
    public List<ProductDto> StockModelList { get; set; }
    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);

    // #######*Constructor*#########
    //     ################
    public AllStockViewModel()
    {
        //DataAccess.Helper.Constants.IP = "192.168.1.103";
        //DataAccess.Helper.Constants.Port = "1433";
        //context = new AppDb();
        StockModelList = new();
        StockModelListTemp = new();
        ProductQueryParam = new();
        repo = new();
        GetStockModelList();
    }

    // #######################################
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
                //StockModelList = StockModelListTemp;
                OnPropertyChanged(nameof(StockModelList));
                return;
            }
            ProductQueryParam.Text = text;
            StockModelList = repo.GetAllProducts(ProductQueryParam);
            //StockModelList = Services.MockData.GetStocksByText(text.ToLower());

            OnPropertyChanged(nameof(StockModelList));
        }
        catch (Exception ex)
        {
            Helpers.CatchingException.PharmaDisplayAlert(ex.Message) ;
        }
    }
    void GetStockModelList()
    {
        StockModelList = repo.GetAllProducts(ProductQueryParam);
        //StockModelList = StockModelListTemp= Services.MockData.GetStocksNonRepet();
    }
}
