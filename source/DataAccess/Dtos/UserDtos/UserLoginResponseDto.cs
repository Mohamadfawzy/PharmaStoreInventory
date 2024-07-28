namespace DataAccess.Dtos.UserDtos;

public class UserLoginResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }

}
