namespace PharmaStoreInventory.Models.User;

public class LoginBothSidesDTO
{
    public string AccountUsername { get; set; } = null!;
    public string AccountPassword { get; set; } = null!;

    public string EmpUsername { get; set; } = null!;
    public string EmpPassword { get; set; } = null!;
    public string? SecretKey { get; set; } = null!;
}
