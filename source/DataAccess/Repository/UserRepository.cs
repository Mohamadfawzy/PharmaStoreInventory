using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.ExtensionMethods;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Repository;

public class UserRepository
{
    readonly PasswordHasher passwordHasher;
    private readonly AppHost context;

    public UserRepository(AppHost context)
    {
        passwordHasher = new PasswordHasher();
        this.context = context;
        // OpenConnection();
    }


    public async Task<Result> CreateAsync(UserRegisterDto userDto)
    {
        if (userDto is null)
            return Result.Failure("User data cannot be null.");

        // Initialize 
        Result result;

        // Convert the UserAccount Dto to UserAccount Entity
        var en = (UserAccount)userDto;

        try
        {
            // Hash the userDto's password
            en.PasswordHash = passwordHasher.HashPassword(userDto.Password);

            // Set the expiration time for the verification code
            en.VCodeExpirationTime = DateTimeOffset.UtcNow.AddMinutes(Helper.Strings.VerificationCodeMinutesExpires);

            // Confirm the userDto's email by default
            en.EmailConfirmed = true;

            // set lockout end 7 day form now
            //userAccount.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7);



            ////Add the new userDto entity to the context
            //var entityEntry = await context.Users.AddAsync(userAccount);

            ////Check if the entity state is "Added" and save the changes to the database
            //if (entityEntry.State == EntityState.Added)
            //{
            //    await context.SaveChangesAsync();
            //    // Return a success result with the userDto's ID
            //    result = Result.Success($"Id:{entityEntry.Entity.Id}");
            //}

            var sql = $" INSERT INTO Users (FullName, PharmacyName, Email, PhoneNumber, DeviceID, PasswordHash ,EmailConfirmed ) " +
                $"VALUES ( N'{en.FullName}',N'{en.PharmacyName}',N'{en.Email}',N'{en.PhoneNumber}',N'{en.DeviceID}', N'{en.PasswordHash}', '{en.EmailConfirmed}' ) ";

            int rowsAffected = await context.Database.ExecuteSqlRawAsync(sql);
            if (rowsAffected > 0)
            {
                result = Result.Success();
            }
            else
            {
                // If the entity state is not "Added", return a failure result with the state
                result = Result.Failure($"Failed to add the userDto. EntityState:");
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


    public async Task<Result> AddUserAsync(UserAccount userAccount)
    {
        try
        {
            var entityEntry = await context.Users.AddAsync(userAccount);

            int rowsAffected = await context.SaveChangesAsync();
            if (rowsAffected > 0)
            {
                return Result.Success($"Id:{entityEntry.Entity.Id}");
            }
            else
            {
                return Result.Failure($"Failed to add the userDto. EntityState:");
            }
        }
        catch (Exception ex)
        {
            var exceptionMessage = $"Message: {ex.Message}\nInnerException: {ex.InnerException?.Message}";
            return Result.Failure(exceptionMessage);
        }
    }

    /// <summary>
    /// Asynchronously checks if a email exists in the users table.
    /// </summary>
    /// <param name="email">The email to check for existence.</param>
    /// <returns>return true if  the email exists.</returns>
    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await context.Users.AsNoTracking().AnyAsync(e => e.Email != null && e.Email == email);
    }


    /// <summary>
    /// Asynchronously checks if a phone number exists in the users table.
    /// </summary>
    /// <param name="phone">The phone number to check for existence.</param>
    /// <returns>return true if  the phone number exists.</returns>
    public async Task<bool> IsPhoneExistAsync(string phone)
    {
        return await context.Users.AsNoTracking().AnyAsync(e => e.PhoneNumber != null && e.PhoneNumber == phone);
    }


    public async Task<UserAccount?> FindUserByEmailAsync(string email)
    {
        try
        {
            return await context.Users
                .FirstOrDefaultAsync(x => x.Email != null && x.Email.Equals(email));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message + nameof(FindUserByEmailAsync));
        }
    }

    public async Task<bool> IsUserActiveAsync(int userId)
    {
        try
        {
            return await context.Users.AnyAsync(x => x.Id == userId && x.IsActive);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message + nameof(FindUserByEmailAsync));
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
        user.IsLoggedIn = true;
        user.AccessFailedCount = 0;
        //userDto.DeviceID = dviceId;
        //userDto.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
    }


    public async Task<Result> UpdateLogoutUserAsync(int id)
    {
        try
        {
            var res = context.Users.FirstOrDefault(x => x.Id == id);
            if (res != null)
            {
                res.IsLoggedIn = false;
                res.LoggedOutAt = DateTimeOffset.UtcNow;
                var row = await context.SaveChangesAsync();
                if (row > 0)
                    return Result.Success("you are logout successfully");
                return Result.Failure("logout Failure");
            }
            return Result.Failure();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorCode.ExceptionError, ex.Message);
        }

        //var q = await context.Users
        //    .Where(b => b.Id == id)
        //    .ExecuteUpdateAsync(setters => setters
        //    .SetProperty(b => b.IsLoggedIn, false)
        //    .SetProperty(b => b.LoggedOutAt, DateTimeOffset.UtcNow));
        //if (q > 0)
        //    return Result.Success("you are logout successfily");
        //return Result.Failure("logout not successfily");
    }


    /// <summary>
    /// Updates the password for a userDto identified by their userDto ID.
    /// </summary>
    /// <param name="userId">The ID of the userDto whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</returns>
    public async Task<Result> UpdatePasswordByUserIdAsync(int userId, string newPassword)
    {
        var query = context.Users.Where(b => b.Id == userId);
        return await UpdatePasswordAsync(query, newPassword);
    }


    /// <summary>
    /// Updates the password for a userDto identified by their email address.
    /// </summary>
    /// <param name="email">The email of the userDto whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</param>
    public async Task<Result> UpdatePasswordByEmailAsync(string email, string newPassword)
    {
        var query = context.Users.Where(b => b.Email == email);
        return await UpdatePasswordAsync(query, newPassword);
    }



    public async Task<UserAccount?> FindUserById(int userId)
    {
        try
        {
            return await context.Users
                .AsNoTracking()
                .Where(id => id.Id == userId)
                .FirstOrDefaultAsync();
        }
        catch
        {
            return null;
        }
    }


    public async Task<Result> UpdateVerificationCode(int userId, string code, DateTimeOffset vCodeExpirationTime)
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


    public async Task<Result> UpdateUserInfoAsync(UserEditNameDto dto)
    {
        try
        {
            // Execute the update
            var rowsAffected = await context.Users
                .Where(p => p.Id == dto.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.FullName, dto.FullName)
                    .SetProperty(b => b.PharmacyName, dto.PharmcyName)
                    );

            // Check if the update was successful
            if (rowsAffected > 0)
            {
                return Result.Success("userDto info updated successfully.");
            }

            return Result.Failure(ErrorCode.OperationFailed, "Failed to update the userDto info.");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return Result.Failure(ErrorCode.OperationFailed, $"An error occurred while updating the userDto info: {ex.Message}");
        }
    }

    #region Admin
    public async Task<Result> UpdateActivationUserAccountAsync(UserAccount user, bool active)
    {
        user.IsActive = active;
        await context.SaveChangesAsync();
        return Result.Success($"active email{user.Email}");
    }

    public async Task<Result> UpdateActivationUserAccountAsyncOld(int userId, bool active)
    {
        var q = await context.Users
            .Where(b => b.Id == userId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.IsActive, active));
        if (q > 0)
            return Result.Success($"Activation of AcountId:{userId} is updated");
        return Result.Failure($"Activation of AcountId:{userId} is not updated");
    }

    public async Task<List<UserInfoDto>> ReadAllUsers(FilterUsersQParam qParam)
    {
        IQueryable<UserAccount> query = context.Users;

        query = query.Where(x => x.Id != 1);

        if (qParam.IsActive is not null)
        {
            query = query.Where(x => x.IsActive == qParam.IsActive);
        }

        if (qParam.EmailConfirmed is not null)
        {
            query = query.Where(x => x.EmailConfirmed == qParam.EmailConfirmed);
        }

        if (qParam.Role is not null)
        {
            query = query.Where(x => x.UserRole == qParam.Role);
        }

        if (qParam.OrderBy != UsersOrderBy.Non)
        {
            switch (qParam.OrderBy)
            {
                case UsersOrderBy.Name:
                    query = query.OrderBy(x => x.FullName);
                    break;
                case UsersOrderBy.NameDescending:
                    query = query.OrderByDescending(x => x.FullName);
                    break;
                case UsersOrderBy.Id:
                    query = query.OrderBy(x => x.Id);
                    break;
                case UsersOrderBy.CreateOn:
                    query = query.OrderBy(x => x.CreateOn);
                    break;
                case UsersOrderBy.CreateOnDescending:
                    query = query.OrderByDescending(x => x.CreateOn);
                    break;
            }
        }

        query = query.Pagination(qParam.Page, qParam.PageSize);

        var users = await query
            .Select(userAccount => (UserInfoDto)userAccount)
            .ToListAsync();
        return users;
    }

    public async Task<Result> AdminConfirmsUserEmail(int userId)
    {
        try
        {
            // Execute the update
            var rowsAffected = await context.Users
                .Where(p => p.Id == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.EmailConfirmed, true)
                    .SetProperty(b => b.VerificationCode, "admin"));

            // Check if the update was successful
            if (rowsAffected > 0)
            {
                return Result.Success("userDto info updated successfully.");
            }

            return Result.Failure(ErrorCode.OperationFailed, "Failed to update the userDto info.");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return Result.Exception(ex.Message);
        }
    }
    
    public async Task<Result> RemoveUser(int userId)
    {
        try
        {
            // Execute the update
            var rowsAffected = await context.Users
                .Where(p => p.Id == userId)
                .ExecuteDeleteAsync();

            // Check if the update was successful
            if (rowsAffected > 0)
            {
                return Result.Success("user is deleted successfully.");
            }

            return Result.Failure(ErrorCode.OperationFailed, "not found this user");
        }
        catch (Exception ex)
        {
            // Log the exception if necessary
            return Result.Exception(ex.Message);
        }
    }
    #endregion



    /// <summary>
    /// Central method to update the password for users based on a given query.
    /// </summary>
    /// <param name="query">The query to identify the userDto(s) whose password is to be updated.</param>
    /// <param name="newPassword">The new password to be set.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure.</param>
    private async Task<Result> UpdatePasswordAsync(IQueryable<UserAccount> query, string newPassword)
    {
        try
        {
            // Hash the new password
            var hashPassword = passwordHasher.HashPassword(newPassword);
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
            return Result.Failure(ErrorCode.OperationFailed, $"An error occurred while updating the password: {ex.Message}");
        }
    }

    private void OpenConnection()
    {
        context.Database.OpenConnection();
    }

}