using DataAccess.Entities;

namespace DataAccess.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public string? Gender { get; set; }
    public string? Mobile { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Active { get; set; }
    public string? Deleted { get; set; }
    public int? JobId { get; set; }

    public static implicit operator EmployeeDto(Employee? arg)
    {
        if (arg == null)
            return null!;

        return new EmployeeDto
        {
            Id = (int)arg.Emp_id,
            Password = arg.Pass,
            Username = arg.Username,
            Deleted = arg.Deleted,
            Active = arg.Active,
            Code = arg.Emp_code,
            Email = arg.Emp_mail,
            NameAr = arg.Emp_name_ar,
            NameEn = arg.Emp_name_en,
            Gender = arg.Emp_gender,
            JobId = (int?)arg.Job_id
        };
    }
}
