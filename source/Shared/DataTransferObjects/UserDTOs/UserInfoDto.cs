

using Entities.Models;

namespace DataTransferObjects.UserDTOs;

public class UserInfoDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsActive { get; set; }
     public string? DeviceID { get; set; } = null;
    public string? DeviceModel { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTimeOffset CreateOn { get; set; }

    public static implicit operator UserInfoDto(UserAccount model)
    {
        return new UserInfoDto
        {
            Id = model.Id,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            AccessFailedCount = model.AccessFailedCount,
            CreateOn = model.CreateOn,
            DeviceID = model.DeviceID,
            DeviceModel = model.DeviceModel,
            EmailConfirmed = model.EmailConfirmed,
            IsActive = model.IsActive,
            PharmcyName = model.PharmacyName
        };
    }
}
