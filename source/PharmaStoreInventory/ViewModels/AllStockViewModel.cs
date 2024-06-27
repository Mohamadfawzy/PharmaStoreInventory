using PharmaStoreInventory.Models;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel
{
    public List<StockModel> StockModelList { get; set; } = null!;
    public AllStockViewModel()
    {
        GetStockModelList();
    }

    void GetStockModelList()
    {
        StockModelList = Services.MockData.StockModelList;
    }
}
