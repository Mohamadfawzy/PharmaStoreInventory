using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Entities;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Models.Parameters;
using PharmaStoreInventory.Services;
using System.Globalization;

namespace PharmaStoreInventory.ViewModels;

public partial class EditProAmountViewModel : ObservableObject
{
    public EditProAmountViewModel(ProductUnitsDto product)
    {
        SelectedProduct = product;
        Init();
        GetTotalAmount();
    }

    [ObservableProperty]
    private int year = 0;
    [ObservableProperty]
    private int month = 0;
    [ObservableProperty]
    private int day = 0;
    [ObservableProperty]
    private DateTime? currentExpiryDate;
    [ObservableProperty]
    bool isExpiryDateFrameInPopupVisible;
    [ObservableProperty]
    private decimal modifiedQuantity = 0;

    [ObservableProperty]
    private int mediumUnitQuantity;
    [ObservableProperty]
    private int largUnitQuantity;
    [ObservableProperty]
    private decimal vendorDiscountRate;
    [ObservableProperty]
    private string price;
    [ObservableProperty]
    private decimal taxPrice;
    [ObservableProperty]
    private string productName;
    [ObservableProperty]
    private string barcode;
    [ObservableProperty]
    private string messageInPopupConfirmEditQuntity = "";

    [ObservableProperty]
    private string? totalQuantitiesProduct;

    [ObservableProperty]
    private string descriptionUnit;

    [ObservableProperty]
    private bool confirmEditQuntityVisible = false;

    [ObservableProperty]
    private bool isBusy = false;
    [ObservableProperty]
    private bool mediumUnitQuantityIsVisible = true;

    [ObservableProperty]
    private bool contentInputTransparent = false;

    private ProductUnitsDto SelectedProduct;
    private ProductDetailsDto ResponseProduct;
    private CreateStartStockDetails CurrnetCreateStartStockDetails;

    public async void Init()
    {
        try
        {
            IsExpiryDateFrameInPopupVisible = SelectedProduct.ProductHasExpire == "1" ? true : false;
            ProductName = SelectedProduct.NameAr;
            if (IsExpiryDateFrameInPopupVisible)
            {
                DateTime dateName = DateTime.Now;
                Year = dateName.Year;
                Month = dateName.Month;
                Day = 1;
            }

            Price = Math.Round(SelectedProduct.Price, 2).ToString(CultureInfo.InvariantCulture);
            VendorDiscountRate = Math.Round(SelectedProduct.VendorDiscountRate, 2);
            TaxPrice = Math.Round(SelectedProduct.TaxPrice, 2);
            LargUnitQuantity = 1;
            Barcode = SelectedProduct.ProductId.ToString();

            DescriptionUnit = $"{SelectedProduct.MediumUnitName} ({SelectedProduct.MediumUnitsPerLarge})";

            MediumUnitQuantityIsVisible = SelectedProduct.MediumUnitsPerLarge == 1 ? false : true;
        }
        catch (Exception ex)
        {
            await Alerts.DisplaySnackBar(ex.Message);
        }
    }

    [RelayCommand]
    public async void Submit()
    {

        try
        {
            IsBusy = true;
            ContentInputTransparent = true;

            var param = new ProductExistParameters
            {
                ProductId = 22,
                StoreId = 1,
                ExpDate = new DateTime(2025, 12, 31)
            };
            decimal.TryParse(Price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newPrice);

            //var prodcut = ApiServices.GetByProductAndStoreAndExpDateAsync(param);
            AggregationExpiryDate();

            //var largUnitQ = int.Parse(LargUnitQuantity);
            //var medimUnitQ = int.Parse(MediumUnitQuantity);

            ModifiedQuantity = PriceConverter.ToDecimalQuantity(LargUnitQuantity, MediumUnitQuantity, (int)SelectedProduct.MediumUnitsPerLarge.Value);

            AppPreferences.StartStockDetailsId++;

            CurrnetCreateStartStockDetails = new CreateStartStockDetails()
            {
                SstockId = AppPreferences.StartStockId,
                DetailsId = AppPreferences.StartStockDetailsId,
                ProductId = SelectedProduct.ProductId,
                StoreId = AppPreferences.StoreId,
                VendorId = 1,
                Quantity = ModifiedQuantity,
                SellPrice = newPrice,
                TaxPrice = TaxPrice,
                VendorDiscountRate = VendorDiscountRate,
                BuyPrice = 0,
                UnitId = SelectedProduct.LargeUnitId,
                UnitChange = 1,
                Notes = $"فاتورة جرد رقم {AppPreferences.StartStockId} من الهاتف",
                EmpId = AppPreferences.LocalDbUserId,
                InType = "1",
                FormType = 6,
            };
            CurrnetCreateStartStockDetails.ExpiryDate = CurrentExpiryDate.HasValue ? CurrentExpiryDate.Value : null;

            var respose = await ApiServices.CreatStartStockDetailsAsync(CurrnetCreateStartStockDetails);

            if (respose == null)
            {
                await Alerts.DisplaySnackBar("لا يوجد رد من الخادم");
                return;
            }

            if (respose.IsSuccess)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
                return;
            }

            if (respose.Data == null)
            {
                Notfication("فقد في البيانات", "لم يتم العثور علي المنتج");
                return;
            }
            ResponseProduct = respose.Data;

            if (respose.ErrorCode == ErrorCode.ExpDateIsExist && SelectedProduct.ProductHasExpire == "0")
            {
                Notfication("المنتج له كمية", "يمكنك تعديل الكمية");
                MessageInPopupConfirmEditQuntity = "يوجد رصيد لهذه المنتج يمكنكك تعديل الكمية";
                ConfirmEditQuntityVisible = true;
            }
            else if (respose.ErrorCode == ErrorCode.ExpDateIsExist)
            {
                Notfication("تاريخ الصلاحية موجود", "يمكنك تعديل الكمية");
                MessageInPopupConfirmEditQuntity = "تاريخ الصلاحية موجود لنفس المنتج يمكننك تعديل الكمية فقط";
                ConfirmEditQuntityVisible = true;
            }
            else
            {
                Notfication("خطأ ما حدث", respose.Message);
            }
        }
        catch (Exception ex)
        {
            Notfication("حدث خطأ غير محدد", ex.Message);
            AppPreferences.StartStockDetailsId--;
            ContentInputTransparent = false;

        }
        finally
        {
            IsBusy = false;
            ContentInputTransparent = ConfirmEditQuntityVisible;
        }
    }

    [RelayCommand]
    public async void EditQuntity()
    {
        try
        {
            IsBusy = true;
            ContentInputTransparent = true;

            var startStockDetails = new UpdateProductQuantity
            {
                Id = ResponseProduct.Id,
                ProductId = ResponseProduct.ProductId,
                OldQuantity = ResponseProduct.Quantity,
                NewQuantity = ModifiedQuantity + ResponseProduct.Quantity,

                ProductUnit1 = ResponseProduct.ProductUnit1,
                Notes = "from post mobile",
                EmpId = AppPreferences.LocalDbUserId.ToString(),

                InType = "0",
                FormType = 0
            };

            startStockDetails.ExpDate = CurrentExpiryDate.HasValue ? CurrentExpiryDate.Value : null;

            var updateStartStockDetails = new UpdateStartStockDetails()
            {
                Create = CurrnetCreateStartStockDetails,
                Update = startStockDetails
            };


            if (CurrnetCreateStartStockDetails.SellPrice != ResponseProduct.SellPrice)
            {
                Notfication("التاريخ موجود مع سعر مختلف ", "غير التاريخ ولو بيوم واحد");
                return;
            }

            var result = await ApiServices.UpdateProductQuantityWithHistoryAsync(updateStartStockDetails);

            if (result != null && result.IsSuccess)
            {
                ConfirmEditQuntityVisible = false;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                Notfication("لم يتم التحديث", "");
            }
        }
        catch (Exception ex)
        {
            Notfication("حدث خطأ غير متوقع", ex.Message);
        }
        finally
        {
            IsBusy = false;
            ConfirmEditQuntityVisible = false;
            ContentInputTransparent = false;
        }
    }

    [RelayCommand]
    public void ToggleConfirmEditQuntity()
    {
        ConfirmEditQuntityVisible = !ConfirmEditQuntityVisible;
        ContentInputTransparent = false;
    }


    private async void GetTotalAmount()
    {
        try
        {
            var result = await ApiServices.GetTotalAmountAsync(AppPreferences.StoreId, SelectedProduct.ProductId);
            
            if (result != null && result.IsSuccess && result.Data != null)
            {
                TotalQuantitiesProduct = result.Data.ToString();

            }
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackBar(ex.Message, 7);
        }
    }
    void AggregationExpiryDate()
    {
        if (Year < 1) return;
        CurrentExpiryDate = new DateTime(Year, Month, Day);
    }

    private void Notfication(string title, string body)
    {
        WeakReferenceMessenger.Default.Send(new NotificationMessage(new ErrorMessage(title, body)));

    }



}