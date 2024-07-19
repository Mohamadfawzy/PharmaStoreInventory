using DataAccess.Entities;

namespace DataAccess.Dtos;

public class EmployeeDto
{
    public int? Id { get; set; }
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
            Id = (int?)arg.emp_id,
            Password = arg.pass,
            Username = arg.username,
            Deleted = arg.deleted,
            Active = arg.active,
            Code = arg.emp_code,
            Email = arg.emp_mail,
            NameAr = arg.emp_name_ar,
            NameEn = arg.emp_name_en,
            Gender = arg.emp_gender,
            JobId = (int?)arg.job_id
        };
    }
}
