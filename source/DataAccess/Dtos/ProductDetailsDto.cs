using DataAccess.Entities;

namespace DataAccess.Dtos;

public class ProductDetailsDto
{
    public int Id { get; set; }
    public decimal? ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryBatchID { get; set; } //counter_id
    public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal? Amount { get; set; }
    public decimal? SellPrice { get; set; }
    public string? IsInventoried { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    public string? CompanyNameAr { get; set; }

    //public decimal BuyPrice { get; set; }
    //public decimal TaxPrice { get; set; }
    //public DateTime InsertDate { get; set; }
    //public string? UnitNameAr { get; set; }
    //public string? SiteFullName { get; set; }

    public static implicit operator ProductDetailsDto?(Product_Amount? item)
    {
        if (item == null) 
            return null;
        return new ProductDetailsDto
        {
            Id = (int)item.pa_id,
            ProductId = item.product_id,
            StoreId = item.store_id,
            ExpiryBatchID = item.counter_id,
            VendorId = item.vendor_id,
            ExpDate = item.exp_date,
            Amount = item.amount,
            SellPrice = item.sell_price,
            IsInventoried = item.Product_update,
            // Map other properties as needed
        };
    }

    //public static implicit operator Product_Amount(ProductDetailsDto item)
    //{
    //    return new Product_Amount
    //    {
    //        product_id = item.ProductId ?? 0,
    //        store_id = item.StoreId ?? 0,
    //        counter_id = item.ExpiryBatchID ?? 0,
    //        vendor_id = item.VendorId,
    //        exp_date = item.ExpDate,
    //        amount = item.Amount,
    //        sell_price = item.SellPrice,
    //        pa_id =item.Id,
    //        Product_update=item.IsInventoried,
    //        // Map other properties as needed
    //    };
    //}
}


