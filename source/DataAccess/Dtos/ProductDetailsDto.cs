using DataAccess.Entities;
namespace DataAccess.Dtos;

public class ProductDetailsDto 
{

    public decimal? ExpiryGroupID { get; set; } //Counter_id
    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public string? IsInventoried { get; set; }
    //public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal Quantity{ get; set; } // Amount
    public decimal? SellPrice { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    //public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    //public string? CompanyNameAr { get; set; }
    public string? ProductHasExpire { get; set; }

    public decimal? LargeUnitId { get; set; }          // product_unit1 - العلبة
    public string? LargeUnitName { get; set; }         // product_unit1 - العلبة
    public decimal? MediumUnitId { get; set; }         // product_unit2 - الشريط
    public string? MediumUnitName { get; set; }        // product_unit2 - الشريط
    public decimal? MediumUnitPrice { get; set; }
    public decimal? MediumUnitsPerLarge { get; set; }  // product_unit1_2 - عدد الشرائط في العلبة

    //public static implicit operator ProductDetailsDto?(Product_Amount? item)
    //{
    //    if (item == null) 
    //        return null;
    //    return new ProductDetailsDto
    //    {
    //        Id = item.Pa_id,
    //        ProductId = item.Product_id,
    //        StoreId = item.Store_id,
    //        ExpiryGroupID = item.Counter_id,
    //        //VendorId = item.Vendor_id,
    //        ExpDate = item.Exp_date,
    //        Quantity = item.Amount,
    //        SellPrice = item.Sell_price,
    //        IsInventoried = item.Product_update,
    //        // Map other properties as needed
    //    };
    //}
}


