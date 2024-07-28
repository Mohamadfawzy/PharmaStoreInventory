using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Repository;

public class AdminRepository
{
    readonly AppHost context;
    readonly PasswordHasher passwordHasher;

    public AdminRepository()
    {
        passwordHasher = new PasswordHasher();
        context = new AppHost();

    }


    public async Task<Result> UpdateActivationUserAccountAsync(int id,bool active)
    {
        var q = await context.Users
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.IsActive, active));
        if (q > 0)
            return Result.Success($"Activation of AcountId:{id} is updated");
        return Result.Failure($"Activation of AcountId:{id} is not updated");
    }
}
