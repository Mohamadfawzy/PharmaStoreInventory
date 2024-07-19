namespace DataAccess.Entities;
public class Companys
{
    public decimal? company_id { get; set; }
    public string? company_code { get; set; }
    public string? co_name_ar { get; set; }
    public string? co_name_en { get; set; }
    public string? mobile { get; set; }
    public string? tel { get; set; }
    public string? address { get; set; }
    public string? deleted { get; set; }
    public string? active { get; set; }
    public DateTime? insert_date { get; set; }
    public string? insert_uid { get; set; }
}