namespace PharmaStoreInventory.Models;

public class LoginModel
{
    public string? Email { get; set; }
    public int Phone { get; set; }
    public string Password { get; set; } = null!;

    public LoginModel(string? email, int phone, string password)
    {
        Email = email;
        Phone = phone;
        Password = password;
    }
}
