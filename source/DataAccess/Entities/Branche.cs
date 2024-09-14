namespace DataAccess.Entities;

public class Branche
{
    public Guid Id { get; set; }
    public string? BrachName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Telephone { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public DateTime CreateOn { get; set; }
    public int UserId { get; set; }
}