﻿namespace DataAccess.DomainModel;
public class UserVerificationEmailModel
{
    public string? Email { get; set; }
    public string VerificationCode { get; set; } = string.Empty;
    public DateTimeOffset? ExpirationTime { get; set; }
}