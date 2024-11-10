namespace PharmaStoreInventory.Models;

public class PharmaVersion
{
    public int? Id { get; set; }
    public string? VersionName { get; set; }
    public string? VersionNumber { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public int CreateBy { get; set; }
}
