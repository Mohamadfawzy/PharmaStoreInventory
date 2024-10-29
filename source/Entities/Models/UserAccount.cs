namespace Entities.Models;

public class UserAccount
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmacyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? DeviceID { get; set; } = null;
    public string? DeviceModel { get; set; } 
    public bool IsLoggedIn { get; set; }
    public bool IsActive { get; set; }
    public int AccessFailedCount { get; set; }
    public char UserRole { get; set; } 

    public bool EmailConfirmed { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? VerificationCode { get; set; }
    public DateTimeOffset? VCodeExpirationTime { get; set; }
    public DateTimeOffset CreateOn { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public DateTimeOffset? LoggedOutAt { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string PasswordHash { get; set; } = null!;
}
