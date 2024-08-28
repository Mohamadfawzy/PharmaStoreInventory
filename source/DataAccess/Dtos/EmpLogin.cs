namespace DataAccess.Dtos;

public class EmpLogin(string username, string password)
{
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
}
