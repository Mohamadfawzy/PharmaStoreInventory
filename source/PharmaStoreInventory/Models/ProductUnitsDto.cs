namespace PharmaStoreInventory.Models;

public class ProductUnitsDto
{
    public decimal ProductId { get; set; }
    public string? InternationalCode { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public decimal Price { get; set; }
    public decimal BuyPrice { get; set; }
    public decimal TaxPrice { get; set; }
    public decimal VendorDiscountRate { get; set; }


    public decimal? LargeUnitId { get; set; }          // product_unit1 - العلبة
    public string? LargeUnitName { get; set; }         // product_unit1 - العلبة
    public decimal? MediumUnitId { get; set; }         // product_unit2 - الشريط
    public string? MediumUnitName { get; set; }        // product_unit2 - الشريط
    public decimal? MediumUnitPrice { get; set; }
    public decimal? MediumUnitsPerLarge { get; set; }  // product_unit1_2 - عدد الشرائط في العلبة
    public string? ProductHasExpire { get; set; }

    public decimal? VendorId { get; set; } = 1;
    public string? VendorName { get; set; }
}
