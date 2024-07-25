using DataAccess.Entities;

namespace DataAccess.Dtos;

public class ProductAmoutInsertDto
{

    public decimal Id { get; set; }
    public decimal? ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryBatchID { get; set; } //counter_id
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
            product_id = item.ProductId ?? 0,
            store_id = item.StoreId ?? 0,
            counter_id = item.ExpiryBatchID ?? 0,
            vendor_id = item.VendorId,
            exp_date = item.ExpDate,
            amount = item.Amount,
            sell_price = item.SellPrice,
            pa_id = (int)item.Id,
            Product_update = item.IsInventoried,
            // Map other properties as needed
        };
    }
}
