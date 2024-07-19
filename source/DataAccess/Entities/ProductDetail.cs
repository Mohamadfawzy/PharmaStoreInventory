using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class ProductDetail
{
    [Column("product_id")]
    public decimal ProductId { get; set; }
    [Column("store_id")]
    public decimal StoreId { get; set; }
    [Column("counter_id")]
    public decimal CounterId { get; set; }
    [Column("amount")]
    public decimal? Amount { get; set; }
    [Column("exp_date")]
    public DateTime? ExpDate { get; set; }
    [Column("vendor_id")]
    public decimal VendorId { get; set; }
    [Column("vendor_name_ar")]
    public string? VendorNameAr { get; set; }
    [Column("buy_price")]
    public decimal BuyPrice { get; set; }
    [Column("sell_price")]
    public decimal SellPrice { get; set; }
    [Column("tax_price")]
    public decimal TaxPrice { get; set; }
    [Column("product_update")]
    public string? ProductUpdate { get; set; }
    [Column("insert_date")]
    public DateTime? InsertDate { get; set; }
    [Column("product_name_ar")]
    public string? ProductNameAr { get; set; }
    [Column("product_name_en")]
    public string? ProductNameEn { get; set; }
    [Column("unit_name_ar")]
    public string? UnitNameAr { get; set; }
    [Column("product_unit1")]
    public decimal? ProductUnit1 { get; set; }
    [Column("site_full_name")]
    public string? SiteFullName { get; set; }
    [Column("co_name_ar")]
    public string? CompanyNameAr { get; set; }
}