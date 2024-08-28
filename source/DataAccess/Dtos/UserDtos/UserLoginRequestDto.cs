namespace DataAccess.Dtos.UserDtos;

public class UserLoginRequestDto
{
    public string EmailOrPhone { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsNewDevice { get; set; } 
    public string DviceId { get; set; } = null!;
}
