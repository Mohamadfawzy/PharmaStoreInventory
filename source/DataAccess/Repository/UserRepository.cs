using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
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

            // Add the new user entity to the context
            var entityEntry = await context.Users.AddAsync(userEntity);

            // Check if the entity state is "Added" and save the changes to the database
            if (entityEntry.State == EntityState.Added)
            {
                await context.SaveChangesAsync();
                // Return a success result with the user's ID
                result = Result.Success(entityEntry.Entity.Id.ToString());
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

    public async Task SetUserAccountIsAccessAsync(UserAccount user)
    {
        user.Access = true;
        user.AccessFailedCount = 0;
        await context.SaveChangesAsync();
    }

    public async Task<Result> UpdateUserAccountIsLockoutAsync(int id)
    {
        var q = await context.Users
            .Where(b => b.Id == id)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.Access, false)
            .SetProperty(b => b.LockoutEnd, DateTimeOffset.UtcNow));
        if (q > 0)
            return Result.Success("you are logout successfily");
        return Result.Failure("logout not successfily");
    }







    public async Task<UserAccount?> FindById(int userId)
    {
        //return await GenericReadById<User>(u => u.Id == userId, null);
        try
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
        catch (Exception)
        {
            return null;
        }
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


    public async Task<bool> UpdateVerificationCode(UserAccount user)
    {
        user.VCodeExpirationTime = DateTimeOffset.Now.AddMinutes(Helper.Constants.VerificationCodeMinutesExpires).UtcDateTime;
        context.Attach(user);
        context.Entry(user).Property(vc => vc.VerificationCode).IsModified = true;
        context.Entry(user).Property(et => et.VCodeExpirationTime).IsModified = true;

        return await context.SaveChangesAsync() > 0 ? true : false;
    }

    public async Task<bool> CheckPassword(string password, string hashedPassword)
    {
        return await Task.FromResult(passwordHasher.VerifyPassword(password, hashedPassword));
    }
}