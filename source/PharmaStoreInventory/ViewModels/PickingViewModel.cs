using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Services;
using System.Globalization;
using System.Windows.Input;
using Product = PharmaStoreInventory.Models.ProductDetailsModel;
namespace PharmaStoreInventory.ViewModels;

public class PickingViewModel : BaseViewModel
{
    //#########*PrivateFields*############
    private bool isEditPopupVisible = false;
    private bool isDateSectionInPopupVisible = true;
    private int year = 0;
    private int month = 0;
    private int day = 0;
    private string nameEn = string.Empty;
    private string nameAr = string.Empty;
    private string popupType = "edit"; // or "editAndCopy"
    private string barcode = "0000";
    private string microUnitQuantity = "0";
    private string modifiedQuantity = "0";
    private DateTime? expiryDateLabel;
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
        FetchStockDetails(_barcode);
    }

    //###########*Properties*###########
    #region Public Properties
    public List<Product> ListOfStoc { get; set; } = [];
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
    public string ModifiedQuantity
    {
        get => modifiedQuantity;
        set => SetProperty(ref modifiedQuantity, value);
    }
    public int Year
    {
        get => year;
        set
        {
            SetProperty(ref year, value);
        }
    }
    public int Month
    {
        get => month;
        set => SetProperty(ref month, value);
    }
    public int Day
    {
        get => day;
        set => SetProperty(ref day, value);
    }
    private DateTime? ExpiryDateLabel;
    public bool IsEditPopupVisible
    {
        get => isEditPopupVisible;
        set => SetProperty(ref isEditPopupVisible, value);
    }
    public bool IsExpiryDateFrameInPopupVisible
    {
        get => isDateSectionInPopupVisible;
        set => SetProperty(ref isDateSectionInPopupVisible, value);
    }
    #endregion

    //############*Commands*############
    #region CommandS
    public ICommand CopyProductCommand => new Command<Product>(ExecuteCopyProduct);
    public ICommand MakeInventoryCommand => new Command<Product>(ExecuteMakeInventory);
    public ICommand SelectionChangedCommand => new Command<Product>(ExecuteSelectionChanged);
    public ICommand SaveChangesCommand => new Command(ExecuteSaveChanges);
    #endregion

    //##############*API*###############
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
            await Alerts.DisplaySnackBar(ex.Message, 7);
        }
        finally
        {
            ActivityIndicatorRunning = false;
            OnPropertyChanged(nameof(ListOfStoc));
        }
    }
    #endregion

    //#########*ExecuteMethods*#########
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
            await Alerts.DisplaySnackBar($"ExecuteMakeInventory: {ex.Message}");
        }

    }
    private void ExecuteCopyProduct(Product stock)
    {

        if (stock == null)
        {
            return;
        }

        SelectedProduct.IsSelected = false;
        stock.IsSelected = true;

        if (stock.ProductHasExpire == "0")
        {
            SendNotification(new ErrorMessage("المنتج ليس له تاريخ صلاحية", "يمكنك تعديل الكمية فقط"));
            return;
        }

        PopupType = "editAndCopy";
        if (stock.ProductHasExpire != "1")
        {
            IsExpiryDateFrameInPopupVisible = false;
        }
        IsEditPopupVisible = true;
        ExpiryDateLabel = stock.ExpDate;
        if (stock.ExpDate != null)
        {
            Year = stock.ExpDate.Value.Year;
            Month = stock.ExpDate.Value.Month;
            Day = stock.ExpDate.Value.Day;
        }
        ModifiedQuantity = (stock.Quantity ?? 0).ToString("F2", CultureInfo.InvariantCulture);
        this.SelectedProduct = stock;
    }
    public async void ExecuteSelectionChanged(Product pro)
    {
        try
        {
            if (pro == null)
            {
                return;
            }

            SelectedProduct.IsSelected = false;
            pro.IsSelected = true;

            IsExpiryDateFrameInPopupVisible = pro.ProductHasExpire == "1" ? true : false;

            PopupType = "edit";
            IsEditPopupVisible = true;
            ExpiryDateLabel = pro.ExpDate;

            if (pro.ExpDate != null)
            {
                Year = pro.ExpDate.Value.Year;
                Month = pro.ExpDate.Value.Month;
                Day = pro.ExpDate.Value.Day;
            }
            ModifiedQuantity = (pro.Quantity ?? 0).ToString("F2", CultureInfo.InvariantCulture);
            this.SelectedProduct = pro;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar(ex.Message);
        }
    }

    private async void ExecuteSaveChanges()
    {
        try
        {
            ActivityIndicatorRunning = true;
            AggregationExpiryDate();
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
            await Alerts.DisplaySnackBar($"{nameof(ExecuteSaveChanges)}:{ex.Message}");
        }
        finally
        {
            ActivityIndicatorRunning = false;

        }
    }
    #endregion

    //###########*Processors*###########
    #region Processors
    void ResetValues()
    {
        NameAr = string.Empty;
        NameEn = string.Empty;
    }
    void AggregationExpiryDate()
    {
        if (Year < 1) return;
        ExpiryDateLabel = new DateTime(Year, Month, Day);
    }
    void SendNotification(ErrorMessage error)
    {
        WeakReferenceMessenger.Default
            .Send(new PickingViewNotification(error));
    }

    void SendNotificationToHideKeyBoard()
    {
        ErrorMessage error = new() { Code = 1 };
        WeakReferenceMessenger.Default
            .Send(new PickingViewNotification(error));
    }
    #endregion

    //#########*Handlers*#########
    #region Handlers
    async void SaveEditProduct()
    {
        decimal.TryParse(ModifiedQuantity, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantityParsed);

        try
        {
            if (SelectedProduct.Quantity == null)
            {
                return;
            }
            var changes = false;
            UpdateProductQuantityDto model = new()
            {
                Id = SelectedProduct.Id,
                ProductId = (int)SelectedProduct.ProductId,
                OldQuantity = SelectedProduct.Quantity.Value,
                NewQuantity = quantityParsed,
                ExpDate = ExpiryDateLabel,
                ProductUnit1 = SelectedProduct.ProductUnit1,
                EmpId = AppPreferences.LocalDbUserId.ToString()
            };

            var notes = new System.Text.StringBuilder("تعديل");


            if (model.NewQuantity != SelectedProduct.Quantity)
            {
                notes.Append(" كمية");
                notes.Append(',');
                changes = true;
            }

            if (model.ExpDate != SelectedProduct.ExpDate)
            {
                notes.Append(" تاريخ صلاحية");
                changes = true;
            }

            if (!changes)
            {
                SendNotification(new ErrorMessage("لا يوجد أي تغيرات", ""));
                return;
            }

            model.Notes = notes.ToString().TrimEnd(',');

            var res = await ApiServices.UpdateQuantity(model);

            if (res != null)
            {
                if (res.IsSuccess)
                {
                    SelectedProduct.Quantity = quantityParsed;
                    SelectedProduct.ExpDate = ExpiryDateLabel;
                    SelectedProduct.IsInventoried = "1";
                    IsEditPopupVisible = false;
                }
                else
                {
                    if (res.ErrorCode == ErrorCode.ExceptionError)
                        SendNotification(new ErrorMessage("Exception error", res.Message));

                    else if (res.ErrorCode == ErrorCode.ItemIsExist)
                        SendNotification(new ErrorMessage("تاريخ صلاحية موجود بالفعل", "يوجد منتج آخر له نفس تاريخ الصلاحية"));

                    else if (res.ErrorCode == ErrorCode.NotFoundById)
                        SendNotification(new ErrorMessage("", ""));

                    else
                        SendNotification(new ErrorMessage("حدث  خطأ غير معروف", res.Message));
                }
            }
            else
            {
                SendNotification(new ErrorMessage("حدثت مشكلة أثناء التعديل", "لا يوجد محتوى لعرضه"));
            }

            SendNotificationToHideKeyBoard();
        }
        catch (Exception ex)
        {
            SendNotification(new ErrorMessage("خطأ عام", ex.Message));
        }
    }

    async void SaveCopyProduct()
    {
        try
        {
            if (ExpiryDateLabel == null)
                return;
            decimal.TryParse(ModifiedQuantity, NumberStyles.Any, CultureInfo.InvariantCulture, out var quantityParsed);

            CopyProductAmoutDto copyPro = new()
            {
                Id = SelectedProduct.Id,
                ProductId = SelectedProduct.ProductId,
                Quantity = quantityParsed,
                ExpDate = ExpiryDateLabel.Value,
                ProductUnit1 = SelectedProduct.ProductUnit1,
                EmpId = AppPreferences.LocalDbUserId.ToString()
            };


            var res = await ApiServices.CopyProductAmount(copyPro);
            if (res == null)
            {
                SendNotification(new ErrorMessage("حدث خطأ تحقق من الاتصال بالسيرفر", ""));
            }
            else if (res.IsSuccess)
            {
                FetchStockDetails(Barcode);
                SendNotification(new ErrorMessage("تم حفظ التغيرات بنجاح", ""));
                IsEditPopupVisible = false;
            }
            else if (res.ErrorCode == ErrorCode.ItemIsExist)
            {
                SendNotification(new ErrorMessage("تاريخ الصلاحية موجود بالفعل", ""));
            }
            else if (res.ErrorCode == ErrorCode.NotFoundById)
            {
                SendNotification(new ErrorMessage("لم يتم العثور علي هذا الصنف", "ربما قام أحد بحذفه منذ قليل"));
            }
            else if (res.ErrorCode == ErrorCode.OperationFailed)
            {
                SendNotification(new ErrorMessage("لم يتم حفظ  هذه الصنف", ""));
            }
            else if (res.ErrorCode == ErrorCode.ExceptionError)
            {
                SendNotification(new ErrorMessage("حدث خطأ داخل السيرفر", ""));
            }
            SendNotificationToHideKeyBoard();
        }
        catch (Exception ex)
        {
            SendNotification(new ErrorMessage("An unknown error occurred. in your app", ex.Message));
        }
    }
    #endregion


}
