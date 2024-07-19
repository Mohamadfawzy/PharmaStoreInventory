namespace DataAccess.Entities;

public class Stores
{
    public decimal? store_id { get; set; }
    public string? store_code { get; set; }
    public string? store_name_ar { get; set; }
    public string? store_name_en { get; set; }
    public string? active { get; set; }
    public DateTime? insert_date { get; set; }
    public string? insert_uid { get; set; }
}
