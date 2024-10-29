namespace DataAccess.DomainModel;

public class AuthModel
{
    public string? Message { get; set; } = string.Empty;

    public List<string>? Roles { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime? ExpiresOn { get; set; }
}
