namespace DataAccess.Entities;

public class Stores
{
    public decimal Store_id { get; set; }
    public string?  Store_code { get; set; }
    public string?  Store_name_ar { get; set; } = string.Empty;
    public string?  Store_name_en { get; set; } = string.Empty;
    public string? Active { get; set; }
    public DateTime? Insert_date { get; set; }
    public string? Insert_uid { get; set; }
}
