using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.Helper;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository;

public class UserRepository
{
    readonly AppHost context;
    readonly PasswordHasher passwordHasher;


    public UserRepository()
    {
        passwordHasher = new PasswordHasher();
        context = new AppHost();
    }


    public async Task<Result> CreateAsync(UserAccountDto userDto)
    {
        if (userDto is null)
            return Result.Failure("User data cannot be null.");

        // Initialize 
        Result result;

        // Convert the UserAccount Dto to UserAccount Entity
        var userEntity = (UserAccount)userDto;

        try
        {
            // Hash the user's password
            userEntity.PasswordHash = passwordHasher.HashPassword(userDto.Password);

            // Set the expiration time for the verification code
            userEntity.VCodeExpirationTime = DateTimeOffset.UtcNow.AddMinutes(Helper.Constants.VerificationCodeMinutesExpires);

            // Confirm the user's email by default
            userEntity.EmailConfirmed = true;

            // set lockout end 7 day form now
            userEntity.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7);

            // Add the new user entity to the context
            var entityEntry = await context.Users.AddAsync(userEntity);

            // Check if the entity state is "Added" and save the changes to the database
            if (entityEntry.State == EntityState.Added)
            {
                await context.SaveChangesAsync();
                // Return a success result with the user's ID
                result = Result.Success($"Id:{entityEntry.Entity.Id.ToString()}");
            }
            else
            {
                // If the entity state is not "Added", return a failure result with the state
                result = Result.Failure($"Failed to add the user. EntityState: {entityEntry.State}");
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


    public Task<bool> IsEmailExistAsync(string email)
    {
        return context.Users
            .AnyAsync(e => e.Email != null && e.Email == email);
    }


    public Task<bool> IsPhoneExistAsync(string phone)
    {
        return context.Users
            .AnyAsync(e => e.PhoneNumber != null && e.PhoneNumber == phone);
    }


    public async Task<UserAccount?> FindUserByEmailAsync(string email)
    {
        try
        {
            return await context.Users
                .FirstOrDefaultAsync(x => x.Email != null && x.Email == email);
        }
        catch
        {
            return null;
        }
    }


    public async Task<UserAccount?> FindUserByPhoneNumberAsync(string phone)
    {
        try
        {
            return await context.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber != null && x.PhoneNumber == phone);
        }
        catch
        {
            return null;
        }
    }


    public async Task IncrementAccessFailedCountAsync(UserAccount user)
    {
        user.AccessFailedCount += 1;
        await context.SaveChangesAsync();
    }


    public async Task SetUserAccountIsAccessAsync(UserAccount user, string deviceId)
    {
        user.IsLoggedIn = true;
        user.AccessFailedCount = 0;
        //user.DeviceID = dviceId;
        //user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
    }


    public async Task<Result> UpdateLogoutUserAsync(int id)
    {
        var q = await context.Users
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.IsLoggedIn, false)
            .SetProperty(b => b.LoggedOutAt, DateTimeOffset.UtcNow));
        if (q > 0)
            return Result.Success("you are logout successfily");
        return Result.Failure("logout not successfily");
    }

    /// <summary>
    /// Updates the password for a user identified by their user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
    public async Task<Result> UpdatePasswordByUserIdAsync(int userId, string newPassword)
    {
        var query = context.Users.Where(b => b.Id == userId);
        return await UpdatePasswordAsync(query, newPassword);
    }

    /// <summary>
    /// Updates the password for a user identified by their email address.
    /// </summary>
    /// <param name="email">The email of the user whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</param>
    public async Task<Result> UpdatePasswordByEmailAsync(string email, string newPassword)
    {
        var query = context.Users.Where(b => b.Email == email);
        return await UpdatePasswordAsync(query, newPassword);
    }

    /// <summary>
    /// Central method to update the password for users based on a given query.
    /// </summary>
    /// <param name="query">The query to identify the user(s) whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</param>
    private async Task<Result> UpdatePasswordAsync(IQueryable<UserAccount> query, string newPassword)
    {
        try
        {
            // Hash the new password
            var hashPassword = passwordHasher.HashPassword(newPassword);
            //query = query.Where(x => !x.IsLoggedIn);
            // Execute the update
            var rowsAffected = await query.ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.PasswordHash, hashPassword));

            // Check if the update was successful
            if (rowsAffected > 0)
            {
                return Result.Success("Password updated successfully.");
            }

            return Result.Failure(ErrorCode.OperationFailed, "Failed to update the password.");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return Result.Failure(ErrorCode.OperationFailed, $"3 An error occurred while updating the password: {ex.Message}");
        }
    }


    public async Task<UserAccount?> FindUserById(int userId)
    {
        try
        {
            return await context.Users.FindAsync(userId);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Result> UpdateVerificationCode(int userId,string code, DateTimeOffset vCodeExpirationTime)
    {
        var q = await context.Users
            .Where(b => b.Id == userId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.VerificationCode, code)
            .SetProperty(b => b.VCodeExpirationTime, vCodeExpirationTime)
            );
        if (q > 0)
            return Result.Success("We have sent you a verification code to your email.");
        return Result.Failure();
    }













    public async Task<UserAccount?> ReadUserIdByEmailAsync(string email)
    {
        try
        {
            return await context.Users
                .Where(x => x.Email == email.ToLower())
                .Select((user) =>
                    new UserAccount
                    {
                        Id = user.Id,
                        Email = user.Email,
                        VerificationCode = user.VerificationCode,
                        VCodeExpirationTime = user.VCodeExpirationTime,
                        FullName = user.FullName
                    })
                .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<UserAccount?> ReadUserIdByPhoneAsync(string phone)
    {
        try
        {
            return await context.Users
                .Where(x => x.PhoneNumber == phone.ToLower())
                .Select((user) => new UserAccount { Id = user.Id, PhoneNumber = user.PhoneNumber, VerificationCode = user.VerificationCode, VCodeExpirationTime = user.VCodeExpirationTime, FullName = user.FullName })
                .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }

    //public async Task<UserVerificationEmailModel?> ReadVerificationCode(string userId)
    //{
    //    Expression<Func<UserAccount, bool>> filter = u => u.Id == userId;

    //    return await GenericReadSingle(filter, (u) => new UserVerificationEmailModel
    //    {
    //        Email = u.Id,
    //        GenerateVerificationCode = u.GenerateVerificationCode,
    //        VCodeExpirationTime = u.VCodeExpirationTime,

    //    });
    //}

    public async Task<bool> CheckPassword(string password, string hashedPassword)
    {
        return await Task.FromResult(passwordHasher.VerifyPassword(password, hashedPassword));
    }
}