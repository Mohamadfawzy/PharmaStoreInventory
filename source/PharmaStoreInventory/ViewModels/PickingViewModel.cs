using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Dtos;
using DataAccess.Repository;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Product = PharmaStoreInventory.Models.ProductDetailsModel; //DataAccess.Dtos.ProductDetailsDto;
namespace PharmaStoreInventory.ViewModels;

public class PickingViewModel : BaseViewModel
{
    //#########*PrivateFields*############
    //private readonly ProductAmountRepo repo;
    private string nameEn = string.Empty;
    private string nameAr = string.Empty;
    private string barcode = "0000";
    private string microUnitQuantity = "0";
    private decimal modifiedQuantity = 0;
    private bool isVisibleEditQuantityAndExpiryPopup = false;
    private string modifiedExpiaryDate = string.Empty;
    private DateTime? expiaryDateLabel;
    private Product SelectedProduct = new();

    //#########*Constructor*############
    //##################################
    public PickingViewModel()
    {
        //repo = Application.Current?.MainPage?.Handler?.MauiContext?.Services.GetService<ProductAmountRepo>()!;
        ListOfStoc = [];
        Task.Run(async () => { await FetchStockDetails(AppValues.NavigationProductCode); });
    }

    //########*PublicProperties*########
    //##################################
    #region Public Properties
    public ObservableCollection<Product> ListOfStoc { get; set; }
    public string NameEn { get => nameEn; set => SetProperty(ref nameEn, value); }
    public string NameAr { get => nameAr; set => SetProperty(ref nameAr, value); }
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
    public decimal ModifiedQuantity
    {
        get => modifiedQuantity;
        set => SetProperty(ref modifiedQuantity, value);
    }
    public string ExpiaryDateAsNumber
    {
        get => modifiedExpiaryDate;
        set => SetProperty(ref modifiedExpiaryDate, value);

    }
    public DateTime? ExpiaryDateLabel
    {
        get => expiaryDateLabel;
        set => SetProperty(ref expiaryDateLabel, value);
    }

    public bool IsVisibleEditQuantityAndExpiryPopup
    {
        get => isVisibleEditQuantityAndExpiryPopup;
        set => SetProperty(ref isVisibleEditQuantityAndExpiryPopup, value);
    }
    #endregion

    //############*CommandS*############
    //##################################
    #region CommandS
    public ICommand CopyProductCommand => new Command<Product>(ExecuteCopyProduct);
    public ICommand MakeInventoryCommand => new Command<Product>(ExecuteMakeInventory);
    public ICommand SelectionChangedCommand => new Command<Product>(ExecuteSelectionChanged);
    public ICommand ExpiaryChangedCommand => new Command<string>(ExecuteExpiaryChanged);
    public ICommand SaveChangesCommand => new Command(ExecuteSaveChanges);
    #endregion

    //############*Fethch*##############
    //##################################
    #region Fethch Data
    public async Task FetchStockDetails(string productCode = "22")
    {
        ActivityIndicatorRunning = true;
        try
        {
            ListOfStoc.Clear();
            var list = await ApiServices.GetProductDetails(false, productCode, AppPreferences.StoreId);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    ListOfStoc.Add(item);
                }
                NameAr = list[0].ProductNameAr ?? "اسم المنتج غير موجود";
                NameEn = list[0].ProductNameEn ?? "Product name not found";
                Barcode = productCode;
                OnPropertyChanged(nameof(ListOfStoc));
            }
            else
            {
                ResetValues();
            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar(ex.Message, 20);
        }
        ActivityIndicatorRunning = false;
    }
    #endregion

    //#########*ExectueMethods*#########
    //##################################
    #region Exectue Methods
    private async void ExecuteMakeInventory(Product item)
    {
        var res = await ApiServices.UpdateInventoryStatus(false, "1", (int)item.ProductId, (int)item.ExpiryGroupID);
        if (res != null && res.IsSuccess)
        {
            item.IsInventoried = "1";
        }
    }
    private void ExecuteCopyProduct(Product stock)
    {
        ListOfStoc.Add(stock);
        //OnPropertyChanged(nameof(ListOfStoc));
    }

    private void ExecuteSelectionChanged(Product dto)
    {
        if (dto != null && dto.Quantity != null)
        {
            IsVisibleEditQuantityAndExpiryPopup = true;
            ModifiedQuantity = dto.Quantity.Value;
            ConvertDateToNumber(dto.ExpDate);
            ExpiaryDateLabel = dto.ExpDate;
            SelectedProduct = dto;
        }
    }

    private void ExecuteExpiaryChanged(string text)
    {
        ConvertNumberToDate(text);
    }
    #endregion

    void ResetValues()
    {
        NameAr = string.Empty;
        NameEn = string.Empty;
        Barcode = string.Empty;
    }

    async void ExecuteSaveChanges()
    {
        //var res = await repo.CanAddExpirationDate((int)SelectedProduct.Id, (int)SelectedProduct.ProductId, ExpiaryDateLabel.Value);
        //res = await repo.UpdateQuantity((int)SelectedProduct.Id, SelectedProduct.Quantity.Value, ModifiedQuantity, ExpiaryDateLabel, AppPreferences.LocalDbUserId.ToString());
        try
        {
            UpdateProductQuantityDto model = new()
            {
                Id = SelectedProduct.Id,
                OldQuantity = SelectedProduct.Quantity.Value,
                NewQuantity = ModifiedQuantity,
                ExpDate = ExpiaryDateLabel.Value,
                EmpId = AppPreferences.LocalDbUserId.ToString()
            };

            var res = await ApiServices.UpdateQuantity(model);
            if (res != null && res.IsSuccess)
            {
                SelectedProduct.Quantity = ModifiedQuantity;
                SelectedProduct.ExpDate = ExpiaryDateLabel;
            }
            else
                await Helpers.Alerts.DisplaySnackbar(res.Message, 20);
            IsVisibleEditQuantityAndExpiryPopup = false;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{nameof(ExecuteSaveChanges)}:{ex.Message}");
        }
    }
    void ConvertDateToNumber(DateTime? dateTime)
    {
        if (dateTime != null)
        {
            ExpiaryDateAsNumber = dateTime.Value.ToString("MMyy");
        }
    }

    void ConvertNumberToDate(string value)
    {
        if (value.Length == 4)
        {
            var month = value[..2];
            var year = $"20{value[2..]}";
            //Do check if month > 12
            ExpiaryDateLabel = new DateTime(int.Parse(year), int.Parse(month), 01);
        }
    }

    #region deleted


    //private void ExecuteOnCameraDetectionFinished(BarcodeResult[] result)
    //{
    //    if (result.Length > 0)
    //    {
    //        Barcode = result[0].DisplayValue;
    //        Task.Run(async () => { await FetchStockDetails(Barcode); });
    //        CameraEnabled = false;
    //        GriRow = 2;
    //    }
    //    else
    //    {
    //        ResetValues();
    //    }
    //}
    //private void ExecuteNewScan()
    //{
    //    CameraEnabled = true;
    //    GriRow = 2;
    //}

    //public void DecimalSplitDto (decimal value)
    //{
    //    var integerPart = (int)value;
    //    var fractionalPart = value - integerPart;
    //    IntegerPart = integerPart;
    //    FractionalPart = fractionalPart;

    //}

    //public int IntegerPart { get; set; }
    //public decimal FractionalPart { get; set; }
    #endregion
}
