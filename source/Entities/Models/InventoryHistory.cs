namespace Entities.Models;
public class InventoryHistory
{
    public decimal Ssut_id { get; set; }
    public decimal? Store_id { get; set; }
    public decimal? Start_emp_id { get; set; }
    public DateTime? Start_time { get; set; }
    public DateTime? End_time { get; set; }
    public decimal? End_emp_id { get; set; }
    public string? Compu_name { get; set; }
}
