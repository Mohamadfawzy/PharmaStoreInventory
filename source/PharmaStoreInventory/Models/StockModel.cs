namespace PharmaStoreInventory.Models;
public class StockModel
{
    public int Id { get; set; }
    public string? ItemNameArabic { get; set; }
    public string? ItemNameEnglish { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public bool IsCounted { get; set; }
    public string? Distributor { get; set; }
    public string Barcode { get; set; }
}
