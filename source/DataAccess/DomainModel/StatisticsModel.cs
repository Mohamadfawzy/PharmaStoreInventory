namespace DataAccess.DomainModel;

public class StatisticsModel
{
    public int TotalProducts { get; set; }
    public int ExpiredProducts { get; set; }
    public int WillExpireIn3Months { get; set; }
    public int InventoryedProducts { get; set; }
}
