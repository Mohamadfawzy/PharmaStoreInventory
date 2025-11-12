using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.Views;

namespace PharmaStoreInventory.ViewModels;

public partial class PickingProductsViewModel : ObservableObject
{
    public PickingProductsViewModel()
    {
        //GetProducts();
    }



    [ObservableProperty]
    private string barcode;
    [ObservableProperty]
    private bool isRefrech;
    [ObservableProperty]
    private ProductUnitsDto currentProduct;
    [ObservableProperty]
    private bool activityIndicatorRunning;
    [ObservableProperty]
    private bool notFoundVisible = false;

    public async void FetchStockDetails(string? productCode)
    {

        if (productCode == null)
        {
            return;
        }
        try
        {
            Barcode = ExtractBarcode(productCode);
            var pagedResult = await ApiServices.GetAllProductsWithUnitsAsync(Barcode, true);

            if (pagedResult.Data == null || pagedResult.Data.FirstOrDefault() == null)
            {
                NotFoundVisible = true;
                return;
            }

            var product = pagedResult.Data.FirstOrDefault();

            await Application.Current.MainPage.Navigation.PushModalAsync(new EditProAmountView(product));

        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar(ex.Message, 7);
        }
        finally
        {
            ActivityIndicatorRunning = false;
            //OnPropertyChanged(nameof(ListOfStoc));
        }
    }

    [RelayCommand]
    public async void Emplement()
    {
        //var result = await AddStartStockHeader();
        //var r = AddStartStockDetails(result);
        //var rss = await UpdateProductQuantityWithHistoryAsync();
    }




    //private async Task<bool> UpdateProductQuantityWithHistoryAsync()
    //{
    //    var startStockDetails = new UpdateProductQuantity
    //    {
    //        Id = 29607,
    //        ProductId = 22,
    //        OldQuantity = 3m,
    //        NewQuantity = 5m,
    //        ExpDate = new DateTime(2033, 3, 3),
    //        ProductUnit1 = 5m,
    //        Notes = "from post mobile",
    //        EmpId = "1001",
    //        InType = "0",
    //        FormType = 0
    //    };

    //    var result = await ApiServices.UpdateProductQuantityWithHistoryAsync(startStockDetails);

    //    await Alerts.DisplaySnackBar(result.IsSuccess.ToString());
    //    return result.IsSuccess;
    //}


    private async Task<ProductDetailsDto> AddStartStockDetails(decimal ssId)
    {
        var startStockDetails = new CreateStartStockDetails
        {
            //Store_id = 1,
            //Sstock_id = ssId,
            //Product_id = 22,
            //VendorId = 1,
            //Exp_date = new DateTime(2025, 12, 31, 0, 0, 0),
            //Amount = 3,
            //Sell_price = 110,
            //Buy_price = 78.7500m,
            //Tax_price = 5.0000m,
            //Unit_id = 5,
            //Unit_change = 1,
            //Notes = "stock initioalized form mobile",
            EmpId = 1001,
            InType = "0",
            FormType = 6
        };

        var result = await ApiServices.CreatStartStockDetailsAsync(startStockDetails);

        await Alerts.DisplaySnackBar(result.Data.ToString());
        return result.Data;
    }

    private async Task<decimal> AddStartStockHeader()
    {
        var startStockHeader = new StartStockHeader
        {
            Store_id = 1,
            Product_number = 0,
            Total_bill = 0m,
            Cashier_id = 1001,
            Notes = "Stock initialized from mobile",
            Insert_uid = 1001,
            Insert_date = new DateTime(2025, 10, 13, 12, 34, 56)
        };


        var result = await ApiServices.CreatStartStockHeaderAsync(startStockHeader);

        await Alerts.DisplaySnackBar(result.Data.ToString());
        return result.Data;
    }

    private string ExtractBarcode(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            string[] words = input.Split('-', '$');
            return words[0];
        }
        return string.Empty;
    }

}
