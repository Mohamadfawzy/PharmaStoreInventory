namespace DataAccess.Entities;
public class Product_Amount
{
    public int? product_id { get; set; }
    public decimal? counter_id { get; set; }
    public decimal? store_id { get; set; }
    public decimal? vendor_id { get; set; }
    public decimal? amount { get; set; }
    public decimal? buy_price { get; set; }
    public decimal? sell_price { get; set; }
    public decimal? tax_price { get; set; }
    public DateTime? exp_date { get; set; }
    public string? insert_uid { get; set; }
    public DateTime? insert_date { get; set; }
    public string? update_uid { get; set; }
    public DateTime? update_date { get; set; }
    public string? Product_update { get; set; } = string.Empty;
    public DateTime? Product_update_date { get; set; }
    public decimal? pa_id { get; set; }
    public string? batch_num { get; set; }
}