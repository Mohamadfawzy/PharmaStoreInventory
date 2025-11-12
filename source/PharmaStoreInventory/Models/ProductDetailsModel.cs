using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;

namespace PharmaStoreInventory.Models;

public class ProductDetailsModel : ObservableObject
{
    decimal? quantity;
    string? formattedQuantity;
    string? isInventoried;
    private DateTime? expDate;

    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryGroupID { get; set; }
    public string? IsInventoried { get => isInventoried; set => SetProperty(ref isInventoried, value); }
    public DateTime? ExpDate { get => expDate; set => SetProperty(ref expDate, value); }
    public decimal? Quantity
    {
        get => quantity;
        set
        {
            SetProperty(ref quantity, value);
            FormattedQuantity = value?.ToString("F3", CultureInfo.InvariantCulture);
        }
    }

    public decimal? SellPrice { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    public string? VendorNameAr { get; set; }
    public string? ProductHasExpire { get; set; }


    // 
    public decimal? LargeUnitId { get; set; }          // product_unit1 - العلبة
    public string? LargeUnitName { get; set; }         // product_unit1 - العلبة
    public decimal? MediumUnitId { get; set; }         // product_unit2 - الشريط
    public string? MediumUnitName { get; set; }        // product_unit2 - الشريط
    public decimal? MediumUnitPrice { get; set; }
    public decimal? MediumUnitsPerLarge { get; set; }  // product_unit1_2 - عدد الشرائط في العلبة


    public string? FormattedPrice => SellPrice?.ToString("F2", CultureInfo.InvariantCulture);
    public string? FormattedQuantity { get => formattedQuantity; set => SetProperty(ref formattedQuantity, value); }

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
