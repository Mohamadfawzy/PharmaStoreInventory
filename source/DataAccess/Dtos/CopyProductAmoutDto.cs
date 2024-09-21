namespace DataAccess.Dtos;

public class CopyProductAmoutDto
{
    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime ExpDate { get; set; }
    public string EmpId { get; set; } = null!;
}
