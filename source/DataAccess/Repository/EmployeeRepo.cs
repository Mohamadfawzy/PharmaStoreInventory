using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;

namespace DataAccess.Repository;
public class EmployeeRepo
{

    private readonly AppDb context;
    public EmployeeRepo(AppDb _context)
    {
        context = _context;
    }

    public Task<EmployeeDto?> Login(string username, string password)
    {
        var result = context.Employee
            .Where(d => d.deleted == ("0"))
            .Where(u => u.username == username)
            .Where(p => p.pass == password)
            .Select(x => (EmployeeDto)x)
            .FirstOrDefaultAsync();
        return result;
    }
}
