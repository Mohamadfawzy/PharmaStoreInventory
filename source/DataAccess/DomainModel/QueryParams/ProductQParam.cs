namespace DataAccess.DomainModel.QueryParams;

public class ProductQParam
{
    public bool IsGroup { get; set; } = true;
    public bool QuantityBiggerThanZero { get; set; } = true;
    //public bool AllowDuplicates { get; set; }
    public string SiteId { get; set; } = string.Empty;
    public int? StoreId { get; set; } = 1;
    public ProductsOrderBy OrderBy { get; set; } = ProductsOrderBy.BiggestPrice;
    public int Page { get; set; } = 1;
    //public int PageSize { get; set; } = 30;
    public string Text { get; set; } = string.Empty;
}
