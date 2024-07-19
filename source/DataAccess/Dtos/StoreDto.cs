namespace DataAccess.Dtos;

public class StoreDto
{
    public decimal? StoreId { get; set; }
    public string? StoreCode { get; set; }
    public string? StoreNameAr { get; set; }
    public string? StoreNameEn { get; set; }
    public string? Active { get; set; }
    public DateTime? InsertDate { get; set; }
    public string? InsertUid { get; set; }
}
