using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Repository;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel : BaseViewModel
{
    //private readonly ProductAmountRepo repo;
    private List<ProductDto> productsListTemp = [];
    private List<ProductDto>? products;
    public List<SortModel> sortListItems =
    [
        new () { Id = 2, IsSelected = false , Name="الاسم"},
        new () { Id = 1, IsSelected = false , Name="كود المنتج", },
        new () { Id = 4, IsSelected = false , Name = "أقل سعر" },
        new () { Id = 3, IsSelected = true , Name = "أعلى سعر" },
        new () { Id = 6, IsSelected = false , Name = "أقل كمية" },
        new () { Id = 5, IsSelected = false  , Name = "أكبر كمية" },
    ];
    private bool bottomSheet = false;
    // #######*Constructor*#########
    //     ################
    public AllStockViewModel()
    {
        //repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        ProductQueryParam = new() { StoreId = AppPreferences.StoreId.ToString() };
        //GetAllProducts();
    }

    // ########*Public*########
    public ProductQParam ProductQueryParam { get; set; }
    public List<ProductDto>? Products
    {
        get => products;
        set => SetProperty(ref products, value);
    }

    public bool BottomSheet
    {
        get => bottomSheet;
        set => SetProperty(ref bottomSheet, value);
    }

    public short SelectedSortBy { get; set; }
    public bool FilterIsGroup { get; set; } = true;
    public bool FilterQuantityBiggerThanZero { get; set; } = true;

    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public ICommand FiltersCommand => new Command(ExecuteFilters);


    public ICommand OpenBottomSheetCommand => new Command(ExecuteOpenBottomSheet);
    public ICommand CloseBottomSheetCommand => new Command(CloseOpenBottomSheet);
    public List<SortModel> SortListItems
    {
        get => sortListItems;
        set => SetProperty(ref sortListItems, value);
    }

    // #######################################
    void ExecuteOpenBottomSheet()
    {
        BottomSheet = true;
    }

   
    private async void SearchBoxTyping(string text)
    {
        if (text == "" || text == " ")
        {
            ProductQueryParam.Text = string.Empty;
            return;
        }

        await GetFromSearch(text);
        ActivityIndicatorRunning = false;
    }


    private async Task GetAllProducts()
    {
        try
        {
            //await Task.Delay(300);
            await Task.Run(async () =>
            {
                var list = await ApiServices.GetAllProducts(ProductQueryParam);
                if (list != null)
                {
                    Products = list;
                    //await Task.Delay(1000);
                    productsListTemp = list;
                }
            });
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar($"{ex.Message}");
        }

        ActivityIndicatorRunning = false;
    }

    private async Task GetFromSearch(string text)
    {
        ActivityIndicatorRunning = true;
        try
        {
            if (string.IsNullOrEmpty(text))
            {
                Products = productsListTemp;
                OnPropertyChanged(nameof(Products));
                return;
            }

            ProductQueryParam.Text = text;
            var list = await ApiServices.GetAllProducts(ProductQueryParam);
            if (list != null)
                Products = list;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message);
        }
    }


    private async void ExecuteFilters()
    {
        ActivityIndicatorRunning = true;
        CloseOpenBottomSheet();

        try
        {
            await Task.Run(async () =>
            {
                ProductQueryParam.OrderBy = (DataAccess.DomainModel.ProductsOrderBy)SelectedSortBy;
                ProductQueryParam.QuantityBiggerThanZero = FilterQuantityBiggerThanZero;
                ProductQueryParam.IsGroup = FilterIsGroup;
                await Task.Delay(250);
                Products = await ApiServices.GetAllProducts(ProductQueryParam).ConfigureAwait(true);
            });
        }
        catch
        {
            await Alerts.DisplaySnackbar("Exception from ExecuteFilters");
        }
        ActivityIndicatorRunning = false;
    }

    // exectued method
    void StoreSelectionChanged(SortModel item)
    {
        var oldItem = SortListItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        SelectedSortBy = item.Id;
    }


    void CloseOpenBottomSheet()
    {
        BottomSheet = false;
    }
}
