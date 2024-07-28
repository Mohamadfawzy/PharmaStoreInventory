namespace DataAccess.DomainModel;

public class ProductQueryParameters
{
    public bool IsGroup { get; set; } = true;
    public bool QuantityBiggerThanZero { get; set; } = true;
    public bool AllowDuplicates { get; set; }
    public string SiteId { get; set; } = string.Empty;
    public string? StoreId { get; set; } = string.Empty;
    public short OrderBy { get; set; } = 1;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 30;
    public string Text { get; set; } = string.Empty;
}
