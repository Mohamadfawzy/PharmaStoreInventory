﻿namespace ApiSettingsManager.Models;

public class UserLoginResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool IsActive { get; set; }
    public char UserRole { get; set; }
}
