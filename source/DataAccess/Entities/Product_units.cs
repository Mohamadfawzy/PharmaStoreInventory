namespace DataAccess.Entities;
public class Product_units
{
    public decimal? unit_id { get; set; }
    public string? unit_code { get; set; }
    public string? unit_name_ar { get; set; }
    public string? unit_name_en { get; set; }
    public string? deleted { get; set; }
    public DateTime? insert_date { get; set; }
    public string? insert_uid { get; set; }
}