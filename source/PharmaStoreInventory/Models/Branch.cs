namespace PharmaStoreInventory.Models;

public class Branch
{
    public Guid Id { get; set; }
    public string BrachName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string IpAdrress { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
}
