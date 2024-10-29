namespace Entities.Models;
public class ProductAmountChange
{
    public decimal Id { get; set; }
    public decimal? Counter_id { get; set; }
    public decimal? Product_id { get; set; }
    public decimal? Store_id { get; set; }
    public decimal? Old_amount { get; set; }
    public decimal? New_amount { get; set; }
    public DateTime? Exp_date { get; set; }
    public int? Form_type { get; set; }
    public decimal? Sell_price { get; set; }
    public decimal? Buy_price { get; set; }
    public decimal? Tax_price { get; set; }
    public decimal? Vendor_id { get; set; }
    public string? Form_notice { get; set; }
    public string? Insert_uid { get; set; } 
    public string? In_type { get; set; }
    public DateTime? Insert_date { get; set; }

}
