namespace Entities.Models;
public class Product_Amount
{
    // Composite Primary Keys (Product_id,Store_id,Counter_id)
    public decimal Product_id { get; set; }
    public decimal Store_id { get; set; }
    public decimal Counter_id { get; set; }
    public decimal Pa_id { get; set; } // IDENTITY
    public decimal? Vendor_id { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Buy_price { get; set; }
    public decimal? Sell_price { get; set; }
    public decimal? Tax_price { get; set; }
    public DateTime? Exp_date { get; set; }
    public string? Insert_uid { get; set; }
    public DateTime? Insert_date { get; set; }
    public string? Update_uid { get; set; }
    public DateTime? Update_date { get; set; }
    public string? Product_update { get; set; }
    public DateTime? Product_update_date { get; set; }
    public string? Batch_num { get; set; }
}