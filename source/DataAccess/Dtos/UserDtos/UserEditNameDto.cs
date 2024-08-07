namespace DataAccess.Dtos.UserDtos;

public class UserEditNameDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PharmcyName { get; set; } = string.Empty;
}
