using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
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

    public List<HasExpiryModel> hasExpiryCollectionItems =
    [
        new () { Id = "1", IsSelected = false , Name="لها صلاحية"},
        new () { Id = "0", IsSelected = false , Name="ليس لها صلاحية"},
        new () { Id = "", IsSelected = true , Name="كلاهما"},
    ];
    private bool bottomSheet = false;
    private int pageSize;
    // #######*Constructor*#########
    //     ################
    public AllStockViewModel()
    {
        //repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        PageSize = AppPreferences.PageSize;
        ProductQueryParam = new() { StoreId = AppPreferences.StoreId.ToString() , PageSize= PageSize};
        //GetAllProducts();
    }

    // ########*Public*########
    public ProductQParam ProductQueryParam { get; set; }
    public List<ProductDto>? Products
    {
        get => products;
        set => SetProperty(ref products, value);
    }
    public NoDataModel NoDataModel =>
     new("nodataicon.png", "No Data", "There is no data to show you right now", true);


    public bool BottomSheet
    {
        get => bottomSheet;
        set => SetProperty(ref bottomSheet, value);
    }

    public int PageSize
    {
        get => pageSize;
        set
        {
            AppPreferences.PageSize = value;
            SetProperty(ref pageSize, value);
        }
    }

    public short SelectedSortBy { get; set; }
    public bool FilterIsGroup { get; set; } = true;
    public bool FilterQuantityBiggerThanZero { get; set; } = true;

    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public ICommand HasExpiryFilterSelectionChangedCommand => new Command<HasExpiryModel>(HasExpiryFilterSelectionChanged);
    public ICommand FiltersCommand => new Command(ExecuteFilters);
    public ICommand OpenBottomSheetCommand => new Command(ExecuteOpenBottomSheet);
    public ICommand CloseBottomSheetCommand => new Command(CloseOpenBottomSheet);
    public ICommand ShowAllProductCommand => new Command(ExecutShowAllProduct);

    public List<SortModel> SortListItems
    {
        get => sortListItems;
        set => SetProperty(ref sortListItems, value);
    }

    public List<HasExpiryModel> HasExpiryCollectionItems
    {
        get => hasExpiryCollectionItems;
        set => SetProperty(ref hasExpiryCollectionItems, value);
    }

    // #######################################
    private async Task GetProducts()
    {
        try
        {
            ProductQueryParam.PageSize = PageSize;
            Products = await ApiServices.GetAllProducts(ProductQueryParam);
            if (Products != null && Products.Count > 0)
            {
                IsNoDataElementVisible = false;
            }
            else IsNoDataElementVisible = true;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar("GetProducts: " + ex.Message);
        }
    }

    void ExecuteOpenBottomSheet()
    {
        BottomSheet = true;
    }

    async void ExecutShowAllProduct()
    {
        ActivityIndicatorRunning = true;
        try
        {
            ProductQueryParam.Text = string.Empty;
            await GetProducts();
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar("ExecutShowAllProduct: " + ex.Message);
        }
        finally
        {
            IsEmptyViewVisible = false;
            ActivityIndicatorRunning = false;
        }

    }

    private async void SearchBoxTyping(string text)
    {
        try
        {
            if (text == "" || text == " ")
            {
                ProductQueryParam.Text = string.Empty;
                return;
            }
            await GetFromSearch(text);
            ActivityIndicatorRunning = false;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar("SearchBoxTyping: " + ex.Message);
        }
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
            await GetProducts();
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar("GetFromSearch: " + ex.Message);
        }
        finally
        {
            IsEmptyViewVisible = false;
            ActivityIndicatorRunning = false;
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
                await GetProducts();
                IsEmptyViewVisible = false;
                ActivityIndicatorRunning = false;
            });
        }
        catch
        {
            await Alerts.DisplaySnackbar("Exception from ExecuteFilters");
        }
    }

    // exectued method
    void StoreSelectionChanged(SortModel item)
    {
        var oldItem = SortListItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        SelectedSortBy = item.Id;
    }

    void HasExpiryFilterSelectionChanged(HasExpiryModel item)
    {
        var oldItem = HasExpiryCollectionItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        ProductQueryParam.HasExpire = item.Id;
    }


    void CloseOpenBottomSheet()
    {
        BottomSheet = false;
    }
}
