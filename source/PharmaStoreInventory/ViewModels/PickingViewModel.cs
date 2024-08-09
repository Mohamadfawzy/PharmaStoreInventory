
using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Dtos;
using DataAccess.Repository;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class PickingViewModel : ObservableObject
{
    private string nameEn = string.Empty;
    private string nameAr = string.Empty;
    private string barcode = "6221068000977";
    private string microUnitQuantity = "0";
    private string majorUnitQuantity = "0";
    private readonly ProductAmountRepo repo;
    /// <summary>
    /// Constructor
    /// </summary>
    public PickingViewModel()
    {
        repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        ListOfStoc = new ObservableCollection<ProductDetailsDto>();
        Task.Run(GetStockDetails);
    }
    #region Properties
    public ObservableCollection<ProductDetailsDto> ListOfStoc { get; set; }

    public string NameEn
    {
        get => nameEn;
        set => SetProperty(ref nameEn, value);
    }
    public string NameAr
    {
        get => nameAr;
        set => SetProperty(ref nameAr, value);
    }
    public string Barcode
    {
        get => barcode;
        set => SetProperty(ref barcode, value);
    }
    public string MicroUnitQuantity
    {
        get => microUnitQuantity;
        set => SetProperty(ref microUnitQuantity, value);
    }
    public string MajorUnitQuantity
    {
        get => majorUnitQuantity;
        set => SetProperty(ref majorUnitQuantity, value);
    }
    #endregion

    public ICommand CopySelectedItemCommand => new Command<ProductDetailsDto>(CopySelectedItem);

    #region Get Data
    public async void GetStockDetails()
    {
        try
        {
            ListOfStoc.Clear();
            var list = await repo.GetProductDetailsByProcedure(-1,"22",1);
           if (list != null)
            {
                foreach (var item in list)
                {
                    ListOfStoc.Add(item);
                }
                NameAr = list[0].ProductNameAr!;
                NameEn = list[0].ProductNameEn!;

            }
            OnPropertyChanged(nameof(ListOfStoc));
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message,20);
        }
    }
    #endregion


    // exectued method
    void CopySelectedItem(ProductDetailsDto stock)
    {
        ListOfStoc.Add(stock);
        //Helpers.CatchingException.DisplayAlert(MajorUnitQuantity);
    }
   

}
