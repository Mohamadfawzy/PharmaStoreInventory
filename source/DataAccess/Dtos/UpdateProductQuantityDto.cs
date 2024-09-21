namespace DataAccess.Dtos;

public class UpdateProductQuantityDto
{
    public decimal Id { get; set; }
    public int ProductId { get; set; }
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    public DateTime? ExpDate { get; set; }
    public string EmpId { get; set; } = null!;
}
