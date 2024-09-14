using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.ExtensionMethods;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;

public class BrancheRepository
{
    private readonly AppHost context;

    public BrancheRepository()
    {
        this.context = new AppHost();
    }

    public async Task<Result> CreateAsync(BrancheDto dto)
    {
        if (dto is null)
            return Result.Failure("Branch data cannot be null.");

        // Initialize 
        Result result;

        // Convert the UserAccount Dto to UserAccount Entity
        var branch = (Branche)dto;

        try
        {
            string insertQuery = $@"
            INSERT INTO Branches 
            (
                Id, BrachName, Username, Password, Telephone, IpAddress, Port, UserId
            )
            VALUES 
            (
                N'{branch.Id}', 
                N'{branch.BrachName}', 
                N'{branch.Username}', 
                N'{branch.Password}', 
                N'{branch.Telephone}', 
                N'{branch.IpAddress}', 
                N'{branch.Port}',
                N'{branch.UserId}' ) ";

            int rowsAffected = await context.Database.ExecuteSqlRawAsync(insertQuery);
            if (rowsAffected > 0)
            {
                result = Result.Success();
            }
            else
            {
                // If the entity state is not "Added", return a failure result with the state
                result = Result.Failure($"Failed to add the dto. EntityState:");
            }
        }
        catch (Exception ex)
        {
            // Capture exception details and return a failure result
            var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
            result = Result.Failure(exceptionMessage);
        }

        // Return the result
        return result;
    }



    public async Task<List<BrancheDto>?> ReadAllBranshes(int userId)
    {
        try
        {
            IQueryable<Branche> query = context.Branches;

            query = query.Where(x => x.UserId == userId);

            var users = await query
                .Select(branche => (BrancheDto)branche)
                .ToListAsync();
            return users;
        }
        catch
        {
            return null;
        }
        
    }
    
    public async Task<Result> DeleteBranshe(Guid brancheId)
    {
        var rowsAffected = await context.Branches
            .Where(x => x.Id == brancheId)
            .ExecuteDeleteAsync();
        if(rowsAffected > 0)
        {
            return Result.Success($"ExecuteDelete Successfully ({rowsAffected} row affected)");
        }
        return Result.Failure("No row affected");
    }
}
