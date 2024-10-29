using Entities.Models;

namespace DataTransferObjects.UserDTOs;

public class UserRegisterDto
{
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
    public string? DeviceID { get; set; } = "emptyFromDto";
    public string? VerificationCode { get; set; }


    public static implicit operator UserAccount(UserRegisterDto dto)
    {
        return new UserAccount()
        {
            FullName = dto.FullName,
            PharmacyName = dto.PharmcyName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DeviceID = dto.DeviceID,
        };
    }
}
