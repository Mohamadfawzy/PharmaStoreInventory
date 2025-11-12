namespace PharmaStoreInventory.Models;

public class StartStockHeader
{
    public decimal? Store_id { get; set; }
    public int? Product_number { get; set; }
    public decimal? Total_bill { get; set; }
    public decimal? Cashier_id { get; set; }
    public string? Notes { get; set; }
    public decimal? Insert_uid { get; set; }
    public DateTime? Insert_date { get; set; }
}
