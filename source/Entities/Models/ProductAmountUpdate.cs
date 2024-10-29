namespace Entities.Models;
public class ProductAmountUpdate
{
    public decimal Id { get; set; }
    public decimal? Product_id { get; set; }
    public decimal? Store_id { get; set; }
    public decimal? Counter_id { get; set; }
    public decimal? Old_amount { get; set; }
    public decimal? New_amount { get; set; }
    public DateTime? New_exp_date { get; set; }
    public DateTime? Old_exp_date { get; set; }
    public decimal? Sell_price { get; set; }
    public decimal? Tax_price { get; set; }
    public decimal? Buy_price { get; set; }
    public DateTime? Store_date { get; set; }
    public decimal? Vendor_id { get; set; }
    public decimal? Product_unit { get; set; }
    public string? Notes { get; set; }
    public string? Insert_uid { get; set; }
    public DateTime? Insert_date { get; set; }
}
