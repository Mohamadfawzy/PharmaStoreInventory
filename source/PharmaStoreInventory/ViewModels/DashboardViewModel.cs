using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class DashboardViewModel : BaseViewModel
{
    //###########*Fields*###############
    #region Private Fields
    private bool isStoresPopupVisible = false;
    private bool confirmNewInventoryVisibility = false;
    private int storeId = 0;
    private int LatestInventoryId = 0;
    private string title = "العنوان";
    private string storeName = "غير محدد";
    private DateOnly latestInventoryDate;
    private SortModel? currentSelectedStore = null;
    #endregion

    //#########*Constructor*############
    public DashboardViewModel()
    {
        storeId = AppPreferences.StoreId;
        Task.Run(OnStart);
    }

    //###########*Properties*###########
    #region Public Properties
    public DateOnly LatestInventoryDate
    {
        get => latestInventoryDate;
        set => SetProperty(ref latestInventoryDate, value);
    }
    public NoDataModel NoDataModel => new("noconnection", "لقد حدث خطأ ما", "نحن نواجه مشاكل في تحميل هذه الصفحة", true, true);
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
    public bool ConfirmNewInventoryExecutionVisibility
    {
        get => confirmNewInventoryVisibility;
        set => SetProperty(ref confirmNewInventoryVisibility, value);
    }
    public bool IsStoresPopupVisible
    {
        get => isStoresPopupVisible;
        set => SetProperty(ref isStoresPopupVisible, value);
    }
    public int CountAllProducts { get; set; }
    public int CountAllExpiredProducts { get; set; }
    public int CountAllProductsWillExpireAfter3Months { get; set; }
    public int CountAllIsInventoryed { get; set; }
    #endregion

    //############*Commands*############
    #region Commands
    public ICommand RefreshCommand => new Command(ExecuteCRefresh);
    public ICommand GotoBranchesViewCommand => new Command(ExecuteGotoBranchesView);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(ExecuteStoreSelectionChanged);
    public ICommand ToggleStoresPopupVisibilityCommand => new Command<string>(ExecuteToggleStoresPopupVisibility);
    public ICommand ToggleConfirmNewInventoryVisibilityCommand => new Command(() => ConfirmNewInventoryExecutionVisibility = !ConfirmNewInventoryExecutionVisibility);
    public ICommand SubmitStoreSelectionChangedCommand => new Command(ExecuteSubmitStoreSelectionChanged);
    public ICommand StartNewInventoryCommand => new Command<string>(ExecuteStartNewInventory);
    public ICommand TryToRefreshCommand => new Command(ExecuteTryToRefresh);
    #endregion

    //##############*API*###############
    #region Fetch Data
    private async Task<bool> TestConnection()
    {
        var isSuccess = await ApiServices.TestConnection();

        if (isSuccess)
        {
            IsPlaceholderVisible = false;
            IsNoDataElementVisible = false;
            return true;
        }
        else
        {
            IsNoDataElementVisible = true;
            IsPlaceholderVisible = false;
            WeakReferenceMessenger.Default
                .Send(new DashboardViewNotification(new ErrorMessage("فشل في الاتصال بالسيرفير", "")));
            return false;
        }
    }
    private async Task GetAllStores()
    {
        var list = await ApiServices.GetAllStores();

        if (list != null && list.Count > 0)
        {
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

            // if the first time register storeId
            if (currentSelectedStore == null)
            {
                currentSelectedStore = StoresModelList[0];
                currentSelectedStore.IsSelected = true;
                StoreName = currentSelectedStore.Name;
                AppPreferences.StoreId = storeId = currentSelectedStore.Id;
            }
        }
    }

    public async Task<bool> IsUserActive()
    {
        var res = await ApiServices.IsUserActiveAsync(AppPreferences.HostUserId);
        if (res != null && res.IsSuccess)
        {
            return true;
        }
        return false;
    }
    private async Task GetLatestInventoryHistory()
    {
        try
        {
            var result = await ApiServices.GetLatestInventoryHistoryAsync(storeId);
            if (result != null && result.Start_time != null)
            {
                LatestInventoryDate = DateOnly.FromDateTime(result.Start_time.Value);
                LatestInventoryId = (int)result.Store_id!;
            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackBar(ex.Message, 7);
        }
    }

    private async Task GetProductCountsAsync()
    {
        try
        {
            StatisticsModel = await ApiServices.GetProductCountsAsync(storeId);
            OnPropertyChanged(nameof(StatisticsModel));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackBar(ex.Message, 7);
        }
    }
    #endregion

    //#########*ExecuteMethods*#########
    #region Exectue Methods
    private async void ExecuteCRefresh()
    {
        IsRefreshing = true;

        if (await TestConnection())
        {
            await OnStart();
        }
        IsRefreshing = false;
    }

    private async void ExecuteTryToRefresh()
    {
        ActivityIndicatorRunning = true;

        await OnStart();

        ActivityIndicatorRunning = false;
    }

    private async void ExecuteGotoBranchesView()
    {
        IsRefreshing = true;
        if (Application.Current != null && Application.Current.MainPage != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.BranchesView());
        }
        IsRefreshing = false;
    }

    private void ExecuteStoreSelectionChanged(SortModel item)
    {
        var oldItem = StoresModelList.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        currentSelectedStore = item;
    }

    private void ExecuteToggleStoresPopupVisibility(string status)
    {
        IsStoresPopupVisible = !IsStoresPopupVisible;
    }

    private void ExecuteSubmitStoreSelectionChanged()
    {
        if (currentSelectedStore != null)
        {
            AppPreferences.StoreId = storeId = currentSelectedStore.Id;
            StoreName = currentSelectedStore.Name;
            IsStoresPopupVisible = false;
            ExecuteCRefresh();
        }
    }

    private async void ExecuteStartNewInventory(string commandParameter)
    {
        try
        {
            if (commandParameter == "init")
            {
                ConfirmNewInventoryExecutionVisibility = true;
            }
            else
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
                        await GetLatestInventoryHistory();
                        CountAllIsInventoryed = 0;
                        OnPropertyChanged(nameof(CountAllIsInventoryed));
                        WeakReferenceMessenger.Default
                        .Send(new DashboardViewNotification(new ErrorMessage("بدأ جرد جديد", "تم بدأ جدرد بتاريخ اليوم")));

                    }
                });
                ConfirmNewInventoryExecutionVisibility = false;
            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackBar(ex.Message, 7);
        }
    }
    #endregion

    //###########*Processors*###########
    #region Processors
    public bool CanBackButtonPressed()
    {
        if (IsStoresPopupVisible)
        {
            IsStoresPopupVisible = false;
            return false;
        }
        return true;
    }

    private async Task OnStart()
    {
        //ActivityIndicatorRunning = true;
        var t0 = TestConnection();
        var t1 = GetAllStores();
        var t2 = GetLatestInventoryHistory();
        var t3 = GetProductCountsAsync();
        await Task.WhenAll(t0, t1, t2, t3);
        //ActivityIndicatorRunning = false;
    }
    #endregion
}