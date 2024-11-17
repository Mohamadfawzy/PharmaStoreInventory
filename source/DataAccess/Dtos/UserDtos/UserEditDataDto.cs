namespace DataAccess.Dtos.UserDtos;

public class UserEditDataDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmacyName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
