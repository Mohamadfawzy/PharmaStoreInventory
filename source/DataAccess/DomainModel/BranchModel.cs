using System.Net;

namespace DataAccess.DomainModel;

public class BranchModel
{
    public Guid Id { get; set; }
    public string BrachName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;

    public string HidIpAddress
    {
        get => $"***.***.***.{IpAddress[(IpAddress.LastIndexOf('.') + 1)..]}";
    }
}
