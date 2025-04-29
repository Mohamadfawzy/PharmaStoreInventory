namespace DataAccess.DomainModel.QueryParams;

public class ProductUpdateAndChangeQParam
{
    public decimal? Old_amount { get; set; }
    public DateTime? OldExp_date { get; set; }
    public string? Notice { get; set; }
    public decimal? Product_unit1 { get; set; }
    public string EmpId { get; set; } = null!;
}
