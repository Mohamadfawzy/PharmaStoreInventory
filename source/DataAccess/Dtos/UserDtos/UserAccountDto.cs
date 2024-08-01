using DataAccess.Entities;

namespace DataAccess.Dtos.UserDtos;

public class UserAccountDto
{
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = null!;
    public string? DeviceID { get; set; }
    

    public static implicit operator UserAccount(UserAccountDto dto)
    {
        return new UserAccount()
        {
            FullName = dto.FullName,
            PharmcyName = dto.PharmcyName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DeviceID = dto.DeviceID,
        };
    }
}
