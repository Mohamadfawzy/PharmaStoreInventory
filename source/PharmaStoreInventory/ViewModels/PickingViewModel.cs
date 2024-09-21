using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Repository;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Product = PharmaStoreInventory.Models.ProductDetailsModel; //DataAccess.Dtos.ProductDetailsDto;
namespace PharmaStoreInventory.ViewModels;

public class PickingViewModel : BaseViewModel
{
    //#########*PrivateFields*############
    private string nameEn = string.Empty;
    private string nameAr = string.Empty;
    private string popupType = "edit";// or "editAndCopy"
    private string barcode = "0000";
    private string microUnitQuantity = "0";
    private decimal modifiedQuantity = 0;
    private bool isEditPopupVisible = false;
    private bool isDateSectionInPopupVisible = true;
    private string modifiedExpiaryDate = string.Empty;
    private DateTime? expiaryDateLabel;
    private Product SelectedProduct = new();
    public List<Product> listOfStoc = [];

    //#########*Constructor_1*############
    public PickingViewModel()
    {
        ActivityIndicatorRunning = false;
    }

    //#########*Constructor_2*############
    public PickingViewModel(string _barcode)
    {
        IsEmptyViewVisible = false;
        Task.Run(() => { FetchStockDetails(_barcode); });
    }

    //########*PublicProperties*########
    #region Public Properties
    public List<Product> ListOfStoc { get; set; } = [];
    //{
    //    get => listOfStoc;
    //    set => SetProperty(ref listOfStoc, value);
    //}
    public string NameEn { get => nameEn; set => SetProperty(ref nameEn, value); }
    public string NameAr { get => nameAr; set => SetProperty(ref nameAr, value); }
    public string PopupType { get => popupType; set => SetProperty(ref popupType, value); }
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
    public bool IsEditPopupVisible
    {
        get => isEditPopupVisible;
        set => SetProperty(ref isEditPopupVisible, value);
    }
    public bool IsExpiryDateFramInPopupVisible
    {
        get => isDateSectionInPopupVisible;
        set => SetProperty(ref isDateSectionInPopupVisible, value);
    }

    #endregion

    //############*CommandS*############
    #region CommandS
    public ICommand CopyProductCommand => new Command<Product>(ExecuteCopyProduct);
    public ICommand MakeInventoryCommand => new Command<Product>(ExecuteMakeInventory);
    public ICommand SelectionChangedCommand => new Command<Product>(ExecuteSelectionChanged);
    public ICommand ExpiryDateChangedCommand => new Command<string>(ExecuteExpiryDateChanged);
    public ICommand SaveChangesCommand => new Command(ExecuteSaveChanges);
    #endregion

    //############*Fethch*##############
    #region Fetch Data
    public async void FetchStockDetails(string? productCode)
    {
        ActivityIndicatorRunning = true;
        IsEmptyViewVisible = false;
        ResetValues();
        if (productCode == null)
        {
            return;
        }
        try
        {
            Barcode = productCode;
            var list = await ApiServices.GetProductDetails(AppValues.ProductHasQuantityOnly, productCode, AppPreferences.StoreId);
            if (list != null && list.Count > 0)
            {
                NameAr = list.First().ProductNameAr ?? "اسم المنتج غير موجود";
                NameEn = list.First().ProductNameEn ?? "Product name not found";
                ListOfStoc = list;
            }
            else
            {
                ResetValues();
                ListOfStoc = listOfStoc;
                IsEmptyViewVisible = true;

            }
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar(ex.Message, 7);
        }
        finally
        {
            ActivityIndicatorRunning = false;
            OnPropertyChanged(nameof(ListOfStoc));
        }
    }
    #endregion

    //#########*ExectueMethods*#########
    #region Exectue Methods
    private async void ExecuteMakeInventory(Product item)
    {
        try
        {
            var res = await ApiServices.UpdateInventoryStatus(false, "1", (int)item.ProductId, (int)item.ExpiryGroupID);
            if (res != null && res.IsSuccess)
            {
                item.IsInventoried = "1";
            }
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar($"ExecuteMakeInventory: {ex.Message}");
        }

    }

    private void ExecuteCopyProduct(Product stock)
    {

        if (stock == null)
        {
            return;
        }

        if (stock.ProductHasExpire == "0")
        {
            ShowNotification(new ErrorMessage("المنتج ليس له تاريخ صلاحية", "يمكنك تعديل الكمية فقط"));
            return;
        }

        PopupType = "editAndCopy";
        if (stock.ProductHasExpire != "1")
        {
            IsExpiryDateFramInPopupVisible = false;
        }
        IsEditPopupVisible = true;
        ConvertDateToNumber(stock.ExpDate);
        ExpiaryDateLabel = stock.ExpDate;
        ModifiedQuantity = stock.Quantity ?? 0;
        this.SelectedProduct = stock;
    }

    public void ExecuteSelectionChanged(Product pro)
    {
        if (pro == null)
        {
            return;
        }

        PopupType = "edit";
        if (pro.ProductHasExpire != "1")
        {
            IsExpiryDateFramInPopupVisible = false;
        }
        IsEditPopupVisible = true;
        ConvertDateToNumber(pro.ExpDate);
        ExpiaryDateLabel = pro.ExpDate;
        ModifiedQuantity = pro.Quantity ?? 0;
        this.SelectedProduct = pro;
    }

    private void ExecuteExpiryDateChanged(string text)
    {
        ConvertNumberToDate(text);
    }

    private async void ExecuteSaveChanges()
    {
        try
        {
            if (PopupType == "edit")
            {
                SaveEditProduct();
            }
            else
            {
                SaveCopyProduct();
            }
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackbar($"{nameof(ExecuteSaveChanges)}:{ex.Message}");
        }
    }
    #endregion

    //#########*PrivateMethods*#########
    async void SaveEditProduct()
    {
        try
        {
            if (SelectedProduct.Quantity == null)
            {
                return;
            }

            UpdateProductQuantityDto model = new()
            {
                Id = SelectedProduct.Id,
                OldQuantity = SelectedProduct.Quantity.Value,
                NewQuantity = ModifiedQuantity,
                ExpDate = ExpiaryDateLabel,
                EmpId = AppPreferences.LocalDbUserId.ToString()
            };

            var res = await ApiServices.UpdateQuantity(model);
            if (res != null && res.IsSuccess)
            {
                SelectedProduct.Quantity = ModifiedQuantity;
                SelectedProduct.ExpDate = ExpiaryDateLabel;
                SelectedProduct.IsInventoried = "1";
                IsEditPopupVisible = false;

            }
            else
                await Alerts.DisplaySnackbar(res.Message, 20);
        }
        catch (Exception)
        {

            throw;
        }

    }

    async void SaveCopyProduct()
    {
        try
        {
            if (ExpiaryDateLabel == null)
                return;

            CopyProductAmoutDto copyPro = new()
            {
                Id = SelectedProduct.Id,
                ProductId = SelectedProduct.ProductId,
                Quantity = ModifiedQuantity,
                ExpDate = ExpiaryDateLabel.Value,
                EmpId = AppPreferences.LocalDbUserId.ToString()
            };


            var res = await ApiServices.CopyProductAmout(copyPro);
            if (res == null)
            {
                ShowNotification(new ErrorMessage("حدث خطأ تحقق من الاتصال بالسيرفر", ""));
            }
            else if (res.IsSuccess)
            {
                FetchStockDetails(Barcode);
                ShowNotification(new ErrorMessage("تم حفظ التغيرات بنجاح", ""));
                IsEditPopupVisible = false;
            }
            else if (res.ErrorCode == ErrorCode.ItemIsExist)
            {
                ShowNotification(new ErrorMessage("تاريخ الصلاحية موجود بالفعل", ""));
            }
            else if (res.ErrorCode == ErrorCode.NotFoundById)
            {
                ShowNotification(new ErrorMessage("لم يتم العثور علي هذا الصنف", "ربما قام أحد بحذفه من قليل"));
            }
            else if (res.ErrorCode == ErrorCode.OperationFailed)
            {
                ShowNotification(new ErrorMessage("لم يتم حفظ  هذه الصنف", ""));
            }
            else if (res.ErrorCode == ErrorCode.ExceptionError)
            {
                ShowNotification(new ErrorMessage("حدث خطأ داخل السيرفر", ""));
            }
        }
        catch (Exception ex)
        {
            ShowNotification(new ErrorMessage("An unknown error occurred. in your app", ex.Message));
        }
    }
    void ResetValues()
    {
        NameAr = string.Empty;
        NameEn = string.Empty;
        //Barcode = string.Empty;
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

    void ShowNotification(ErrorMessage error)
    {
        WeakReferenceMessenger.Default
            .Send(new PickingViewNotification(error));
        //WeakReferenceMessenger.Default.Send(new PickingViewNotification(new ErrorMessage("حدث خطأ داخل السيرفر", "")));
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
