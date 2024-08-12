
using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Dtos;
using DataAccess.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Product = DataAccess.Dtos.ProductDetailsDto;
namespace PharmaStoreInventory.ViewModels;

public class PickingViewModel : ObservableObject
{
    private readonly ProductAmountRepo repo;
    private string nameEn = string.Empty;
    private string nameAr = string.Empty;
    private string barcode = "6221068000977";
    private string microUnitQuantity = "0";
    private decimal? majorUnitQuantity = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    public PickingViewModel()
    {
        repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        ListOfStoc = [];
        Task.Run(FetchStockDetails);
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

    public decimal? ModifiedQuantity
    {
        get => majorUnitQuantity;
        set => SetProperty(ref majorUnitQuantity, value);
    }
    #endregion

    public ICommand CopyProductCommand => new Command<Product>(ExecuteCopyProduct);
    public ICommand SelectionChangedCommand => new Command<Product>(ExecuteSelectionChanged);

    #region Get Data
    public async void FetchStockDetails()
    {
        try
        {
            ListOfStoc.Clear();
            var list = await repo.GetProductDetailsByProcedure(-1, "22", 1);
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
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 20);
        }
    }
    #endregion


    // exectued method
   private void ExecuteCopyProduct(Product stock)
    {
        
        ListOfStoc.Add(stock);
        //Helpers.CatchingException.DisplayAlert(ModifiedQuantity);
    }

    private void ExecuteSelectionChanged(Product dto)
    {
        if (dto != null && dto.Quantity != null)
        {
            ModifiedQuantity = dto.Quantity;
        }
    }


    //public void DecimalSplitDto (decimal value)
    //{
    //    var integerPart = (int)value;
    //    var fractionalPart = value - integerPart;
    //    IntegerPart = integerPart;
    //    FractionalPart = fractionalPart;

    //}

    //public int IntegerPart { get; set; }
    //public decimal FractionalPart { get; set; }
}
