namespace DataAccess.DomainModel;

public class ChangePasswordRequest
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmNewPassword { get; set; } = null!;
}
