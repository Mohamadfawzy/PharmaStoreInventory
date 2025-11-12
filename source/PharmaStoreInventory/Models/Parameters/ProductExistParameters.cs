namespace PharmaStoreInventory.Models.Parameters;

public class ProductExistParameters
{
    public ProductExistParameters(decimal productId, decimal storeId, DateTime expDate)
    {
        ProductId = productId;
        StoreId = storeId;
        ExpDate = expDate;
    }

    public ProductExistParameters()
    {

    }

    public decimal ProductId { get; set; }
    public decimal StoreId { get; set; }
    public DateTime ExpDate { get; set; }
}
