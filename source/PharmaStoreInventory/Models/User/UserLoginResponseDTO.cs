﻿using DataAccess.Dtos;

namespace PharmaStoreInventory.Models.User;

public class UserLoginResponseDTO
{
    public bool IsSuccess { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime? ExpiresIn { get; set; }
    public string TokenType { get; set; } = string.Empty;
    public List<string>? Roles { get; set; }
    public UserInfoDTO? User { get; set; }
    public EmployeeDto? Employee { get; set; }
    public string? Message { get; set; } = string.Empty;
}
