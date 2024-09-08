using DataAccess.DomainModel;
using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class DashboardViewModel : BaseViewModel//, IRecipient<DeleteItemMessage>
{
    // private readonly CommonsRepo commonRepo;
    //private readonly ProductAmountRepo productRepo;
    private int storeId = 0;
    private DateOnly latestInventoryDate;
    private SortModel? currentSelectedStore = null;
    private string title = "العنوان";
    private string storeName = "غير محدد";
    private int LatestInventoryId = 0;
    private bool isStoresPopupVisible = false;
    private bool isPlaceholderElementVisible = true;

    // #######*Constructor*#########
    //     ################
    public DashboardViewModel()
    {
        storeId = AppPreferences.StoreId;
        OnStart();
    }

    private async void OnStart()
    {
        ActivityIndicatorRunning = true;
        IsEmptyColleciton = true;
        var t1 = GetAllStores();
        var t2 = GetLatestInventoryHistory();
        var t3 = GetProductCountsAsync();
        await Task.WhenAll(t1, t2, t3);
        ActivityIndicatorRunning = false;
    }

    // ########*Public Properties*########
    #region Properties
    public DateOnly LatestInventoryDate
    {
        get => latestInventoryDate;
        set => SetProperty(ref latestInventoryDate, value);
    }
    public ObservableCollection<SortModel> StoresModelList { get; set; } = [];
    public StatisticsModel? StatisticsModel { get; set; }

    public string StoreName
    {
        get => storeName;
        set => SetProperty(ref storeName, value);
    }
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public bool IsStoresPopupVisible
    {
        get => isStoresPopupVisible;
        set => SetProperty(ref isStoresPopupVisible, value);
    }

    public bool IsPlaceholderElementVisible
    {
        get => isPlaceholderElementVisible;
        set => SetProperty(ref isPlaceholderElementVisible, value);
    }
    public int CountAllProducts { get; set; }
    public int CountAllExpiredProducts { get; set; }
    public int CountAllProductsWillExpireAfter3Months { get; set; }
    public int CountAllIsInventoryed { get; set; }
    #endregion


    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public ICommand StartNewInventoryCommand => new Command(StartNewInventory);
    public ICommand RefreshCommand => new Command(ExecuteCRefresh);
    public ICommand ToggleStoresPopupVisibilityCommand => new Command<string>(ExecuteToggleStoresPopupVisibility);
    public ICommand SubmitStoreSelectionChangedCommand => new Command(ExecuteSubmitStoreSelectionChanged);


    #region Get Data
    private async Task GetAllStores()
    {
        var list = await ApiServices.GetAllStores();

        if (list != null && list.Count > 0)
        {
            IsEmptyColleciton = false;
            StoresModelList.Clear();
            foreach (var model in list)
            {
                if (model != null)
                {
                    var item = new SortModel()
                    {
                        Id = (short)model.Store_id,
                        Name = model.Store_name_ar ?? (model.Store_name_en ?? "not found name")
                    };

                    if (item.Id == storeId)
                    {
                        item.IsSelected = true;
                        StoreName = item.Name;
                        currentSelectedStore = item;
                    }
                    StoresModelList.Add(item);
                }
            }

            // if the first time registr storeId
            if (currentSelectedStore == null)
            {
                currentSelectedStore = StoresModelList[0];
                currentSelectedStore.IsSelected = true;
                StoreName = currentSelectedStore.Name;
                AppPreferences.StoreId = storeId = currentSelectedStore.Id;
            }
        }
    }

    private async Task GetLatestInventoryHistory()
    {
        try
        {
            var result = await ApiServices.GetLatestInventoryHistoryAsync(storeId);
            if (result != null && result.Start_time != null)
            {
                IsEmptyColleciton = false;
                LatestInventoryDate = DateOnly.FromDateTime(result.Start_time.Value);
                LatestInventoryId = (int)result.Store_id!;
            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async Task GetProductCountsAsync()
    {
        try
        {
            StatisticsModel = await ApiServices.GetProductCountsAsync(storeId);
            if (StatisticsModel != null)
            {
                IsEmptyColleciton = false;
                OnPropertyChanged(nameof(StatisticsModel));
            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    private async void StartNewInventory()
    {
        try
        {
            await Task.Run(async () =>
            {
                var model = new StartNewInventoryHistoryDto()
                {
                    EmpId = AppPreferences.LocalDbUserId,
                    LatestHistoryId = LatestInventoryId,
                    StoreId = storeId,
                };

                var result = await ApiServices.StartNewInventoryAsync(model);
                if (result != null && result.IsSuccess)
                {
                    await Helpers.Alerts.DisplaySnackbar("Start New Inventory Success", 7);
                    await GetLatestInventoryHistory();
                    CountAllIsInventoryed = 0;
                    OnPropertyChanged(nameof(CountAllIsInventoryed));
                }
            });
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
        currentSelectedStore = item;
        //AppPreferences.StoreId = storeId = item.Id;
        //StoreName = item.Name;
    }

    void ExecuteSubmitStoreSelectionChanged()
    {
        if (currentSelectedStore != null)
        {
            AppPreferences.StoreId = storeId = currentSelectedStore.Id;
            StoreName = currentSelectedStore.Name;
            IsStoresPopupVisible = false;
            ExecuteCRefresh();
        }
    }

    void ExecuteCRefresh()
    {
        OnStart();
        IsRefreshing = false;
    }

    void ExecuteToggleStoresPopupVisibility(string status)
    {
        IsStoresPopupVisible = !IsStoresPopupVisible;
    }
    public void Receive(NotificationMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //string message = m.Message;

            //if (message.Value == "Message")
            //{
            //    StatisticsModel.InventoryedProducts = 47734;
            //    OnPropertyChanged(nameof(StatisticsModel));
            //}
        });
    }


    #region Deleted
    /*
    private async Task GetCountAllProducts()
    {
        try
        {
            //CountAllProducts = await productRepo.GetCountAllProductsAsync(storeId);
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
            //CountAllExpiredProducts = await productRepo.GetCountAllExpiredProducts(storeId);
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
            //CountAllProductsWillExpireAfter3Months = await productRepo.GetCountAllProductsWillExpireAfter3Months(storeId);
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
            //CountAllIsInventoryed = await productRepo.GetCountAllIsInventoryed(storeId);
            OnPropertyChanged(nameof(CountAllIsInventoryed));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 7);
        }
    }

    */
    #endregion

}