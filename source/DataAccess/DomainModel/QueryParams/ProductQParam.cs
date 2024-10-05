namespace DataAccess.DomainModel.QueryParams;

public class ProductQParam
{
    public string StoreId { get; set; } = string.Empty;
    public bool IsGroup { get; set; } = true;
    public bool QuantityBiggerThanZero { get; set; } = true;
    public string HasExpire { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public ProductsOrderBy OrderBy { get; set; } = ProductsOrderBy.BiggestPrice;
    public int Page { get; set; } = 1;
    public string? SiteId { get; set; } = null;
    public int PageSize { get; set; } = 50;
}
