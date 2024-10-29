namespace PharmaStoreInventory.Models;

public class LoginModel(string? email, int phone, string password)
{
    public string? Email { get; set; } = email;
    public int Phone { get; set; } = phone;
    public string Password { get; set; } = password;
}
