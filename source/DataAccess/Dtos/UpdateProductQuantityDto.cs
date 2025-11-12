namespace DataAccess.Dtos;

public class UpdateProductQuantityDto
{
    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    public DateTime? ExpDate { get; set; }
    public string EmpId { get; set; } = null!;
    public decimal? ProductUnit1 { get; set; }
    public string? Notes { get; set; }
    public string InType { get; set; } = string.Empty;
    public int FormType { get; set; }
}
