using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Contexts;
using DataAccess.DomainModel;

namespace DataAccess.Repository;
public class EmployeeRepo
{

    private readonly AppDb context;
    public EmployeeRepo()
    {
        context = new();
    }

    public async Task<Result<EmployeeDto?>> EmpLogin(EmpLogin emp)
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
            return Result<EmployeeDto?>.Failure("not founeded this username");

        }
        catch (Exception ex)
        {
            return Result<EmployeeDto?>.Failure($"Error: {ex.Message}");
        }
    }
}
