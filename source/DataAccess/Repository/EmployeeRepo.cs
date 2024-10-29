using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Contexts;
using DataAccess.DomainModel;

namespace DataAccess.Repository;
public class EmployeeRepo(AppDb context)
{
    private readonly AppDb context = context;

    public async Task<Result<EmployeeDto?>> EmpLogin(LoginDto emp)
    {
        try
        {
            if (emp == null || string.IsNullOrEmpty(emp.Username) || string.IsNullOrEmpty(emp.Password))
            {
                return Result<EmployeeDto?>.Failure("Check the inputs please");
            }

            var employee = await context.Employee
                  .Where(d => d.Deleted == ("0"))
                  .Where(u => u.Username == emp.Username)
                  .Where(p => p.Pass == emp.Password)
                  .Select(x => (EmployeeDto)x)
                  .FirstOrDefaultAsync();
            if (employee != null)
            {
                return Result<EmployeeDto?>.Success(employee);
            }
            return Result<EmployeeDto?>.Failure("not founded this username");

        }
        catch (Exception ex)
        {
            return Result<EmployeeDto?>.Failure($"Error: {ex.Message}");
        }
    }


    public async Task<Result<string>> TestConnection()
    {
        try
        {
            var employees = await context.Employee
                .Select(x => new { emp_id = x.Emp_id })
                .Take(10)
                .ToListAsync();

            if (employees != null && employees.Count > 0)
            {
                return Result<string>.Success("successfully");
            }
            return Result<string>.Failure("not founded this username");

        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Error: {ex.Message}");
        }
    }
}
