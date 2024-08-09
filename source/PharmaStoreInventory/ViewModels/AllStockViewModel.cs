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
    private ProductAmountRepo repo;

    // ########*Public*########
    public ProductQParam ProductQueryParam { get; set; }
    public List<ProductDto> StockModelListTemp;
    public List<ProductDto> StockModelList { get; set; }
    public ICommand SearchBoxTypingCommand => new Command<string>(SearchBoxTyping);
    public ICommand StoreSelectionChangedCommand => new Command<SortModel>(StoreSelectionChanged);
    public List<SortModel> SortListItems { get; set; }

    // #######*Constructor*#########
    //     ################
    public AllStockViewModel()
    {
        //DataAccess.Helper.Constants.IP = "192.168.1.103";
        //DataAccess.Helper.Constants.Port = "1433";
        repo =  Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        //repo = new();
        StockModelList = new();
        StockModelListTemp = new();
        ProductQueryParam = new();
        Select();
        Task.Run(GetStockModelList);
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
                StockModelList = StockModelListTemp;
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
            await Helpers.Alerts.DisplayAlert(ex.Message);
        }
    }
    private async void GetStockModelList()
    {
        StockModelList = StockModelListTemp = await repo.GetAllProducts(ProductQueryParam);
        OnPropertyChanged(nameof(StockModelList));
    }



    // exectued method
    void StoreSelectionChanged(SortModel item)
    {
        var oldItem = SortListItems.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
    }

    void Select()
    {
        SortListItems = new List<SortModel>()
        {
            new () { Id = 1, IsSelected = false , Name="الاسم"},
            new () { Id = 2, IsSelected = false , Name="كود المنتج", },
            new () { Id = 3, IsSelected = false , Name="أقل سعر"},
            new () { Id = 4, IsSelected = false , Name="أعلى سعر"},
            new () { Id = 5, IsSelected = true , Name="أقل كمية"},
            new () { Id = 6, IsSelected = false , Name="أكبر كمية"},
        };
        OnPropertyChanged(nameof(StockModelList));
    }
}
