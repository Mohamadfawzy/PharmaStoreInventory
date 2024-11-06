using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel : BaseViewModel
{
    //###########*Fields*###############
    #region Private Fields
    private bool bottomSheet = false;
    private int pageSize;
    private List<ProductDto>? products;
    private List<ProductDto> productsListTemp = [];
    private List<SortModel> sortListItems =
    [
        new () { Id = 2, IsSelected = false , Name="الاسم"},
        new () { Id = 1, IsSelected = false , Name="كود المنتج", },
        new () { Id = 4, IsSelected = false , Name = "أقل سعر" },
        new () { Id = 3, IsSelected = true , Name = "أعلى سعر" },
        new () { Id = 6, IsSelected = false , Name = "أقل كمية" },
        new () { Id = 5, IsSelected = false  , Name = "أكبر كمية" },
    ];

    private List<HasExpiryModel> hasExpiryCollectionItems =
    [
        new () { Id = "1", IsSelected = false , Name="لها صلاحية"},
        new () { Id = "0", IsSelected = false , Name="ليس لها صلاحية"},
        new () { Id = "", IsSelected = true , Name="كلاهما"},
    ];

    #endregion

    //#########*Constructor*############
    public AllStockViewModel()
    {
        PageSize = 20;
        ProductQueryParam = new() { StoreId = AppPreferences.StoreId.ToString(), PageSize = PageSize };
    }

    //###########*Properties*###########
    #region Public Properties
    public ProductQParam ProductQueryParam { get; set; }
    public List<ProductDto>? Products
    {
        get => products;
        set => SetProperty(ref products, value);
    }
    public NoDataModel NoDataModel =>
     new("nodataicon.png", "لا يوجد أصناف", "لم نعثر على أي صنف يمكنك البحث  بالاسم او الكود", true);
    public bool BottomSheet
    {
        get => bottomSheet;
        set => SetProperty(ref bottomSheet, value);
    }
    public int PageSize
    {
        get => pageSize;
        set => SetProperty(ref pageSize, value);
    }
    public short SelectedSortBy { get; set; }
    public bool FilterIsGroup { get; set; } = true;
    public bool FilterQuantityBiggerThanZero { get; set; } = true;
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
    #endregion

    //############*Commands*############
    #region Commands
    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public ICommand HasExpiryFilterSelectionChangedCommand => new Command<HasExpiryModel>(HasExpiryFilterSelectionChanged);
    public ICommand FiltersCommand => new Command(ExecuteFilters);
    public ICommand OpenBottomSheetCommand => new Command(ExecuteOpenBottomSheet);
    public ICommand CloseBottomSheetCommand => new Command(CloseOpenBottomSheet);
    public ICommand ShowAllProductCommand => new Command(ExecuteShowAllProduct);
    #endregion

    //##############*API*###############
    #region Fetch Data
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
            await Alerts.DisplaySnackBar("GetProducts: " + ex.Message);
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
            await Helpers.Alerts.DisplaySnackBar("GetFromSearch: " + ex.Message);
        }
        finally
        {
            IsEmptyViewVisible = false;
            ActivityIndicatorRunning = false;
        }
    }
    #endregion

    //#########*ExecuteMethods*#########
    #region Exectue Methods
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
            await Alerts.DisplaySnackBar("SearchBoxTyping: " + ex.Message);
        }
    }
    private void StoreSelectionChanged(SortModel item)
    {
        var oldItem = SortListItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        SelectedSortBy = item.Id;
    }
    private void HasExpiryFilterSelectionChanged(HasExpiryModel item)
    {
        var oldItem = HasExpiryCollectionItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        ProductQueryParam.HasExpire = item.Id;
    }
    private async void ExecuteFilters()
    {
        ActivityIndicatorRunning = true;
        CloseOpenBottomSheet();
        try
        {
            ProductQueryParam.OrderBy = (DataAccess.DomainModel.ProductsOrderBy)SelectedSortBy;
            ProductQueryParam.QuantityBiggerThanZero = FilterQuantityBiggerThanZero;
            ProductQueryParam.IsGroup = FilterIsGroup;
            await Task.Delay(250);
            await GetProducts();
            IsEmptyViewVisible = false;
        }
        catch
        {
            await Alerts.DisplaySnackBar("Exception from ExecuteFilters");
        }
        finally
        {
            ActivityIndicatorRunning = false;
        }
    }
    private void ExecuteOpenBottomSheet()
    {
        BottomSheet = true;
    }
    public void CloseOpenBottomSheet()
    {
        BottomSheet = false;
    }
    private async void ExecuteShowAllProduct()
    {
        ActivityIndicatorRunning = true;
        try
        {
            ProductQueryParam.Text = string.Empty;
            await GetProducts();
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar("ExecuteShowAllProduct: " + ex.Message);
        }
        finally
        {
            IsEmptyViewVisible = false;
            ActivityIndicatorRunning = false;
        }

    }
    #endregion
}
