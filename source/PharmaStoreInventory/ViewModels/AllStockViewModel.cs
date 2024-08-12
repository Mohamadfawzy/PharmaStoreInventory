using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Repository;
using PharmaStoreInventory.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel : ObservableObject
{
    private readonly ProductAmountRepo repo;
    private List<ProductDto> stockModelListTemp = [];
    public List<SortModel> sortListItems =
    [
        new () { Id = 1, IsSelected = false , Name="الاسم"},
        new () { Id = 2, IsSelected = false , Name="كود المنتج", },
        new() { Id = 3, IsSelected = false, Name = "أقل سعر" },
        new() { Id = 4, IsSelected = false, Name = "أعلى سعر" },
        new() { Id = 5, IsSelected = true, Name = "أقل كمية" },
        new() { Id = 6, IsSelected = false, Name = "أكبر كمية" },
    ];


    // #######*Constructor*#########
    //     ################
    public AllStockViewModel()
    {
        repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        StockModelList = [];
        ProductQueryParam = new();
        Task.Run(GetStockModelList);
    }

    // ########*Public*########
    public ProductQParam ProductQueryParam { get; set; }
    public List<ProductDto> StockModelList { get; set; }
    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public List<SortModel> SortListItems
    {
        get => sortListItems;
        set=> SetProperty(ref sortListItems, value);
    }

    // #######################################
    private async void SearchBoxTyping(string text)
    {
        await GetFromSearch(text);
    }

    private async Task GetFromSearch(string text)
    {
        try
        {
            if (string.IsNullOrEmpty(text))
            {
                StockModelList = stockModelListTemp;
                OnPropertyChanged(nameof(StockModelList));
                return;
            }
            ProductQueryParam.Text = text;
            StockModelList = await repo.GetAllProducts(ProductQueryParam);
            //StockModelList = Services.MockData.GetStocksByText(text.ToLower());

            OnPropertyChanged(nameof(StockModelList));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message);
        }
    }
    private async void GetStockModelList()
    {
        StockModelList = stockModelListTemp = await repo.GetAllProducts(ProductQueryParam);
        OnPropertyChanged(nameof(StockModelList));
    }



    // exectued method
    void StoreSelectionChanged(SortModel item)
    {
        var oldItem = SortListItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
    }

}
