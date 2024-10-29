using System.Diagnostics;
using System.Globalization;

namespace DataAccess.Dtos;

public class ProductDto
{
    //public decimal ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string? InternationalCode { get; set; }
    public string? Name { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? StoreId { get; set; }
    public string? HasExpire { get; set; }

    public string? FormattedPrice =>  SalePrice?.ToString("F2", CultureInfo.InvariantCulture);
    public string? FormattedQuantity => Quantity?.ToString("F3", CultureInfo.InvariantCulture);
}
