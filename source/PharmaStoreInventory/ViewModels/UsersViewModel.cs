using CommunityToolkit.Maui.Core.Views;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos.UserDtos;
using PharmaStoreInventory.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class UsersViewModel : BaseViewModel
{
    //#########*PrivateFields*############
    private bool enableNewUsersTab = true;

    //#########*Constructor*############
    public UsersViewModel()
    {
        FilterUsersQParam = new();
        Users = [];
        Task.Run(FetchAllUsersAsync);
    }

    //########*PublicProperties*########
    #region Public Properties
    public FilterUsersQParam FilterUsersQParam { get; set; }
    public ObservableCollection<UserInfoDto> Users { get; set; }
    public bool EnableNewUsersTab
    {
        get => enableNewUsersTab;
        set => SetProperty(ref enableNewUsersTab, value);
    }
    #endregion

    //############*CommandS*############
    #region CommandS
    public ICommand NewUsersTabCommand => new Command<string>(ExecuteNewUsersTab);
    public ICommand ActiveAcountCommand => new Command<UserInfoDto>(ExecuteActiveAcount);
    #endregion

    //############*Fethch*##############
    #region Fethch Data
    private async Task FetchAllUsersAsync()
    {
        ActivityIndicatorRunning = true;
        IsEmptyColleciton = false;
        try
        {
            Users.Clear();
            var list = await ApiServices.GetAllUsersAsync(FilterUsersQParam);
            if (list != null && list.Count > 0)
            {
                IsEmptyColleciton = false;
                foreach (var user in list)
                {
                    Users.Add(user);// = list;
                }
                //OnPropertyChanged(nameof(Users));
            }
            else
                IsEmptyColleciton = true;


        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{ex.Message}");
        }
        ActivityIndicatorRunning = false;
    }
    #endregion

    //#########*ExectueMethods*#########
    #region Exectue Methods
    async void ExecuteNewUsersTab(string cparam)
    {
        if (cparam == "1")
        {
            EnableNewUsersTab = true;
            FilterUsersQParam.IsActive = false;
            await FetchAllUsersAsync();
        }
        else
        {
            EnableNewUsersTab = false;
            FilterUsersQParam.IsActive = true;
            await FetchAllUsersAsync();
        }
    }

    async void ExecuteActiveAcount(UserInfoDto model)
    {
        ActivityIndicatorRunning = true;
        
        var res = await ApiServices.ChangeUserStatus(model.Id);

        if (res == null)
        {
            await Helpers.Alerts.DisplaySnackbar("response is null");
            return;
        }
        if (res.IsSuccess)
        {
            await Helpers.Alerts.DisplaySnackbar(res.Message);
            Users.Remove(model);
            //Users.RemoveAt(Users.IndexOf(model));
            //OnPropertyChanged(nameof(Users));
        }
        else
            await Helpers.Alerts.DisplaySnackbar(res.Message);


        if (Users.Count < 1)
        {
            IsEmptyColleciton = true;
        }
        ActivityIndicatorRunning = false;

    }
    #endregion
}
