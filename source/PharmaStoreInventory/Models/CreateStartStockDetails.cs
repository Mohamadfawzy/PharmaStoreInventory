namespace PharmaStoreInventory.Models;

public class CreateStartStockDetails
{
    public decimal SstockId { get; set; }
    public decimal DetailsId { get; set; }

    public decimal ProductId { get; set; }
    public decimal StoreId { get; set; }
    public decimal VendorId { get; set; }


    public DateTime? ExpiryDate { get; set; }
    public decimal Quantity { get; set; }

    public decimal SellPrice { get; set; }
    public decimal TaxPrice { get; set; }
    public decimal VendorDiscountRate { get; set; }
    public decimal BuyPrice { get; set; }

    public decimal? UnitId { get; set; }
    public decimal? UnitChange { get; set; }

    public string? Notes { get; set; }
    public decimal EmpId { get; set; }
    public string? InType { get; set; }
    public int? FormType { get; set; }
}
