

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using PharmaStoreInventory.Views;
using System.Collections.ObjectModel;

namespace PharmaStoreInventory.ViewModels;

public partial class ProductSearchViewModel : ObservableObject
{
    public ProductSearchViewModel()
    {

    }

    [ObservableProperty]
    private List<ProductUnitsDto>? products;

    [ObservableProperty]
    private bool isNoDataElementVisible = false;

    public NoDataModel NoDataModel =>
     new("empty_state_image.svg", "لا يوجد أصناف", "لم نعثر على أي صنف يمكنك البحث  بالاسم او الكود", false);

    [RelayCommand]
    public async void SearchBoxTyping(string text)
    {
        await GetFromSearch(text);
    }

    [RelayCommand]
    private async void OnAddProduct(ProductUnitsDto product)
    {
        if (product == null)
            return;

        await Application.Current.MainPage.Navigation.PushModalAsync(new EditProAmountView(product));
        //await Alerts.DisplaySnackBar("GetProducts: "+ product.NameAr);
    }

    private async Task GetFromSearch(string text)
    {
        try
        {
            var pagedResult = await ApiServices.GetAllProductsWithUnitsAsync(text, false);
            Products = pagedResult.Data;


            //Products = new();
            //if (pagedResult.Data == null)
            //    return;
            //foreach(var item in pagedResult.Data)
            //{
            //    item.Price = Math.Round(item.Price, 3);
            //    Products.Add(item);
            //}

            if (Products != null && Products.Count > 0)
            {
                IsNoDataElementVisible = false;
            }
            else
                IsNoDataElementVisible = true;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar("GetProducts: " + ex.Message);
        }
    }
}
