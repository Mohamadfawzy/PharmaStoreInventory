
using CommunityToolkit.Mvvm.ComponentModel;
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
    public ObservableCollection<StockModel> ListOfStoc { get; set; }
    public ICommand CopySelectedItemCommand => new Command<StockModel>(CopySelectedItem);

    //Constructor
    public PickingViewModel()
    {
        ListOfStoc = new ObservableCollection<StockModel>();
        GetStockDetails(Barcode);
    }

    void CopySelectedItem(StockModel stock)
    {
        ListOfStoc.Add(stock);
        //Helpers.CatchingException.DisplayAlert(MajorUnitQuantity);
    }
    public void GetStockDetails(string code)
    {
        try
        {
            ListOfStoc.Clear();
            var list = Services.MockData.GetStockByBarcode(code);
            foreach (var item in list)
            {
                ListOfStoc.Add(item);
            }

            NameAr = list.FirstOrDefault()?.ItemNameArabic!;
            NameEn = list.FirstOrDefault()?.ItemNameEnglish!;
            Barcode = code;
            //OnPropertyChanged(nameof(ListOfStoc));
        }
        catch (Exception ex)
        {
            Helpers.Alerts.DisplayAlert(ex.Message);
        }
    }

}
