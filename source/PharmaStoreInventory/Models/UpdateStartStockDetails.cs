namespace PharmaStoreInventory.Models;

public class UpdateStartStockDetails
{
    public CreateStartStockDetails Create { get; set; } = null!;
    public UpdateProductQuantity Update { get; set; } = null!;
}
