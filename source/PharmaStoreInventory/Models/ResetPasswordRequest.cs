namespace PharmaStoreInventory.Models;
public class ResetPasswordRequest
{
    public Guid EVCID { get; set; }
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Password { get; set; } = null!;
}