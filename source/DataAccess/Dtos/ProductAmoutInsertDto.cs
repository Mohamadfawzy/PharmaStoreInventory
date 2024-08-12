using DataAccess.Entities;

namespace DataAccess.Dtos;

public class ProductAmoutInsertDto
{

    public decimal Id { get; set; }
    public decimal? ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryBatchID { get; set; } //Counter_id
    public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal? Amount { get; set; }
    public decimal? SellPrice { get; set; }
    public string? IsInventoried { get; set; }
    public decimal? ProductUnit1 { get; set; }
    public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    public string? CompanyNameAr { get; set; }

    public static implicit operator Product_Amount(ProductAmoutInsertDto item)
    {
        return new Product_Amount
        {
            Product_id = item.ProductId ?? 0,
            Store_id = item.StoreId ?? 0,
            Counter_id = item.ExpiryBatchID ?? 0,
            Vendor_id = item.VendorId,
            Exp_date = item.ExpDate,
            Amount = item.Amount,
            Sell_price = item.SellPrice,
            Pa_id = (int)item.Id,
            Product_update = item.IsInventoried,
            // Map other properties as needed
        };
    }
}
