using DataAccess.Entities;

namespace DataAccess.Dtos;

public class ProductDetailsDto
{
    public decimal Id { get; set; }
    public decimal? ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryGroupID { get; set; } //Counter_id
    public string? IsInventoried { get; set; }
    //public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal? Quantity { get; set; } // Amount
    public decimal? SellPrice { get; set; }
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
            Id = item.Pa_id,
            ProductId = item.Product_id,
            StoreId = item.Store_id,
            ExpiryGroupID = item.Counter_id,
            //VendorId = item.Vendor_id,
            ExpDate = item.Exp_date,
            Quantity = item.Amount,
            SellPrice = item.Sell_price,
            IsInventoried = item.Product_update,
            // Map other properties as needed
        };
    }

    //public static implicit operator Product_Amount(ProductDetailsDto item)
    //{
    //    return new Product_Amount
    //    {
    //        Product_id = item.ProductId ?? 0,
    //        Store_id = item.StoreId ?? 0,
    //        Counter_id = item.ExpiryGroupID ?? 0,
    //        Vendor_id = item.VendorId,
    //        Exp_date = item.ExpDate,
    //        Amount = item.Quantity,
    //        Sell_price = item.SellPrice,
    //        Pa_id =item.Id,
    //        Product_update=item.IsInventoried,
    //        // Map other properties as needed
    //    };
    //}
}


