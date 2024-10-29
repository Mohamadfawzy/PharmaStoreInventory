using AdminApp.Models;
using AdminApp.Services.ApiServices;
using DataTransferObjects.UserDTOs;
using Shared.RequestFeatures;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AdminApp.ViewModels;

public class UsersViewModel : BaseViewModel
{
    //#########*PrivateFields*############
    private bool enableNewUsersTab = true;
    private bool isLoading = false;
    private UserParameters User_Parameters { get; set; }
    private List<FilterOption> usersFilterOption =
    [
        new () { Id = 1, IsSelected = true , Name="تفعيل حساب"},
        new () { Id = 2, IsSelected = false , Name="تأكيد ايميل"},
        new () { Id = 3, IsSelected = false , Name="الموظفين"},
        new () { Id = 4, IsSelected = false , Name="كل العملاء"},
    ];

    //#########*Constructor*############
    public UsersViewModel()
    {
        User_Parameters = new();
        User_Parameters.IsActive = false;
        Init();
    }

    protected async void Init()
    {
        User_Parameters.PageSize = 10;
        User_Parameters.Role = 'C';
        await FetchUsersAsync();
    }

    //########*PublicProperties*########
    #region Public Properties
    public ObservableCollection<UserInfoDto> Users { get; private set; } = [];

    bool isLoadMoreButtonVisible = true;
    public bool IsLoadMoreButtonVisible
    {
        get => isLoadMoreButtonVisible;
        set => SetProperty(ref isLoadMoreButtonVisible, value);
    }
    //public ObservableCollection<UserInfoDto> Users { get; set; }
    public bool EnableNewUsersTab
    {
        get => enableNewUsersTab;
        set => SetProperty(ref enableNewUsersTab, value);
    }
    public List<FilterOption> UsersFilterOption
    {
        get => usersFilterOption;
        set => SetProperty(ref usersFilterOption, value);
    }
    #endregion

    //############*CommandS*############
    #region Commands
    public ICommand NewUsersTabCommand => new Command<string>(ExecuteNewUsersTab);
    public ICommand ActiveAccountCommand => new Command<UserInfoDto>(ExecuteActiveAccount);
    public ICommand LoadMoreDataCommand => new Command(GetNextPageOfData);
    public ICommand RefreshCommand => new Command(Refresh);
    public ICommand UsersFilterOptionSelectionChangedCommand => new Command<FilterOption>(ExecuteUsersFilterOptionSelectionChanged);

    #endregion

    //############*Fetch*##############
    #region Fethch Data
    private async Task FetchUsersAsync()
    {
        try
        {
            IsRefreshing = true;
            IsPlaceholderVisible = false;
            User_Parameters.Page = 1;
            Users.Clear();
            var list = await UserApiService.GetUsersAsync(User_Parameters);
            if (list != null && list.Count > 0)
            {
                IsPlaceholderVisible = false;
                foreach (var user in list)
                {
                    Users.Add(user);
                }
            }
            else
                IsPlaceholderVisible = true;
        }
        catch (Exception ex)
        {
            //await Helpers.Alerts.DisplaySnackbar($"{ex.Message}");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public async Task FetchNextPageOfUsersAsync()
    {
        try
        {
            ActivityIndicatorRunning = true;
            
            isLoading = true;
            var list = await UserApiService.GetUsersAsync(User_Parameters);
            if (list != null && list.Count > 0)
            {
                foreach (var user in list)
                {
                    Users.Add(user);
                }
                IsLoadMoreButtonVisible = true;
            }
            else
            {
                IsLoadMoreButtonVisible = false;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ActivityIndicatorRunning = false;
            isLoading = false;
            //Debug.WriteLine($"\n\n\nPage:{User_Parameters.Page}");
        }
    }
    #endregion

    //#########*ExecuteMethods*#########
    #region Exectue Methods
    async void ExecuteUsersFilterOptionSelectionChanged(FilterOption item)
    {
        var oldItem = UsersFilterOption.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
        HandelUser_Parameters(item.Id);
        User_Parameters.Page = 1;
        Users.Clear();
        await FetchNextPageOfUsersAsync();
    }

    private async void GetNextPageOfData(object obj)
    {
        User_Parameters.Page += 1;
        await FetchNextPageOfUsersAsync();
        //Debug.WriteLine($"\n\n\nPage:{User_Parameters.Page}");
    }

    private async void Refresh(object obj)
    {
        await FetchUsersAsync();
        IsLoadMoreButtonVisible = true;
    }

    async void ExecuteNewUsersTab(string cparam)
    {
        if (cparam == "1")
        {
            EnableNewUsersTab = true;
            User_Parameters.IsActive = false;
            await FetchUsersAsync();
        }
        else
        {
            EnableNewUsersTab = false;
            User_Parameters.IsActive = true;
            await FetchUsersAsync();
        }
    }

    async void ExecuteActiveAccount(UserInfoDto model)
    {
        ActivityIndicatorRunning = true;

        var res = await UserApiService.ChangeUserStatus(model.Id);

        if (res == null)
        {
            //await Helpers.Alerts.DisplaySnackbar("response is null");
            return;
        }
        if (res.IsSuccess)
        {
            //await Helpers.Alerts.DisplaySnackbar(res.Message);
            Users.Remove(model);
            //Users.RemoveAt(Users.IndexOf(model));
            //OnPropertyChanged(nameof(Users));
        }
        else
        //await Helpers.Alerts.DisplaySnackbar(res.Message);


        if (Users.Count < 1)
        {
            IsPlaceholderVisible = true;
        }
        ActivityIndicatorRunning = false;

    }
    #endregion

    void HandelUser_Parameters(short filterOptionId)
    {
        User_Parameters = new();
        switch (filterOptionId)
        {
            case 1:
                User_Parameters.IsActive = false;
                User_Parameters.Role = 'C';
                break;
            case 2:
                User_Parameters.EmailConfirmed = false;
                User_Parameters.Role = 'C';
                break;
            case 3:
                User_Parameters.Role = 'E';
                break;
            case 4:
                User_Parameters.Role = 'C';
                break;
        }
    }
}
