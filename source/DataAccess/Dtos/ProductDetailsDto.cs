namespace DataAccess.Dtos;

public class ProductDetailsDto
{
    public decimal? ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? CounterId { get; set; }
    public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal? Amount { get; set; }
    public decimal? SellPrice { get; set; }
    public string? IsInventoried { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    public string? CompanyNameAr { get; set; }

    //public decimal BuyPrice { get; set; }
    //public decimal TaxPrice { get; set; }
    //public DateTime InsertDate { get; set; }
    //public string? UnitNameAr { get; set; }
    //public string? SiteFullName { get; set; }
}
