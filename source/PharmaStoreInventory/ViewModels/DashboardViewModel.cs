using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Repository;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class DashboardViewModel : ObservableObject
{
    private readonly CommonsRepo commonRepo;
    private readonly ProductAmountRepo productRepo;
    private DateOnly latestInventoryDate;
    private string title = "العنوان";
    private string storeName = "غير محدد";
    private string number = "124568";
    private int storeId = 1;
    private int allProductsCount = 1;
    private int LatestInventoryId = 0;

    // #######*Constructor*#########
    //     ################
    public DashboardViewModel()
    {
        commonRepo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<CommonsRepo>()!;
        productRepo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        StoresModelList = new List<SortModel>();
        storeId = AppPreferences.StoreId;
        Task.Run(OnStart);
    }

    private async Task OnStart()
    {
        //var t1 = Task.Delay(4000);
        await GetLatestInventoryHistory();
        await GetAllStores();
        await GetCountAllProducts();
        await GetCountAllExpiredProducts();
        await GetCountAllProductsWillExpireAfter3Months();
        await GetCountAllIsInventoryed();
        //await Task.WhenAll(t1, t2,t3);
    }

    // ########*Public Properties*########
    #region Properties
    public DateOnly LatestInventoryDate
    {
        get => latestInventoryDate;
        set => SetProperty(ref latestInventoryDate, value);
    }
    public string StoreName
    {
        get => storeName;
        set => SetProperty(ref storeName, value);
    }
    public List<SortModel> StoresModelList { get; set; }
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }
    public int CountAllProducts { get; set; }
    public int CountAllExpiredProducts { get; set; }
    public int CountAllProductsWillExpireAfter3Months { get; set; }
    public int CountAllIsInventoryed { get; set; }
    #endregion


    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public ICommand StartNewInventoryCommand => new Command(StartNewInventory);


    #region Get Data
    async Task GetAllStores()
    {
        var list = await commonRepo.GetAllStores();

        if (list != null)
        {
            foreach (var model in list)
            {
                if (model != null)
                {
                    var item = new SortModel()
                    {
                        Id = (int)model.Store_id,
                        Name = string.IsNullOrEmpty(model.Store_name_ar) ? model.Store_name_en : model.Store_name_ar
                    };
                    StoresModelList.Add(item);
                }
            }
            Helpers.AppPreferences.StoreId = StoresModelList[0].Id;
            StoresModelList[0].IsSelected = true;
            StoreName = StoresModelList[0].Name;
        }
    }

    private async Task GetLatestInventoryHistory()
    {
        try
        {
            var result = await commonRepo.GetLatestInventoryHistoryAsync(storeId);
            if (result != null && result.Start_time != null)
            {
                LatestInventoryDate = DateOnly.FromDateTime(result.Start_time.Value);
                LatestInventoryId = (int)result.Store_id!;
            }

        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async Task GetCountAllProducts()
    {
        try
        {
            CountAllProducts = await productRepo.GetCountAllProducts(storeId);
            OnPropertyChanged(nameof(CountAllProducts));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async Task GetCountAllExpiredProducts()
    {
        try
        {
            CountAllExpiredProducts = await productRepo.GetCountAllExpiredProducts(storeId);
            OnPropertyChanged(nameof(CountAllExpiredProducts));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async Task GetCountAllProductsWillExpireAfter3Months()
    {
        try
        {
            CountAllProductsWillExpireAfter3Months = await productRepo.GetCountAllProductsWillExpireAfter3Months(storeId);
            OnPropertyChanged(nameof(CountAllProductsWillExpireAfter3Months));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async Task GetCountAllIsInventoryed()
    {
        try
        {
            CountAllIsInventoryed = await productRepo.GetCountAllIsInventoryed(storeId);
            OnPropertyChanged(nameof(CountAllIsInventoryed));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }
    #endregion

    // exectued method
    void StoreSelectionChanged(SortModel item)
    {
        var oldItem = StoresModelList.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;

        Helpers.AppPreferences.StoreId = item.Id;
        StoreName = item.Name;
    }

    private async void StartNewInventory()
    {
        try
        {
            await Task.Run(async () =>
            {
                var result = await commonRepo.StartNewInventoryAsync(LatestInventoryId, AppPreferences.LocalDbUserId, storeId);
                if (result != null && result.IsSuccess)
                {
                    await Helpers.Alerts.DisplaySnackbar("Start New Inventory Success", 7);
                    await GetLatestInventoryHistory();
                    await GetCountAllIsInventoryed();
                }
            });
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }
}