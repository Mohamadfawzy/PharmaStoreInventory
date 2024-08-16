using CommunityToolkit.Mvvm.ComponentModel;
using PharmaStoreInventory.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class StockDetailsViewModel : ObservableObject
{
    //private string nameEn = string.Empty;
    //private string nameAr = string.Empty;
    //private string barcode = string.Empty;

    //public string NameEn
    //{
    //    get => nameEn;
    //    set => SetProperty(ref nameEn, value);
    //}
    
    //public string NameAr
    //{
    //    get => nameAr;
    //    set => SetProperty(ref nameAr, value);
    //}
    
    //public string NavigationProductCode
    //{
    //    get => barcode;
    //    set => SetProperty(ref barcode, value);
    //}
    //public ObservableCollection<StockModel> ListOfStoc {  get; set; }
    //public ICommand CopySelectedItemCommand => new Command<StockModel>(CopySelectedItem);
    
    ////Constructor
    //public StockDetailsViewModel(string code)
    //{
    //    ListOfStoc = new ObservableCollection<StockModel>();
    //    GetStockDetails(code);
    //}

    //void CopySelectedItem(StockModel stock)
    //{
    //    ListOfStoc.Add(stock);
    //}
    //async void GetStockDetails(string code)
    //{
    //    try
    //    {
    //        //var list = Services.MockData.GetStockByBarcode(code);
    //        //foreach (var item in list)
    //        //{
    //        //    ListOfStoc.Add(item);
    //        //}

    //        //NameAr = list.FirstOrDefault()?.ItemNameArabic!;
    //        //NameEn = list.FirstOrDefault()?.ItemNameEnglish!;
    //        //NavigationProductCode = code;
    //    }
    //    catch (Exception ex)
    //    {
    //        await Helpers.Alerts.DisplaySnackbar(ex.Message);
    //    }
    //}



}
