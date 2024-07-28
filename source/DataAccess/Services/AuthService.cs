using DataAccess.DomainModel;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess.Services;

public class AuthService
{
    private readonly Repository.UserRepository repo;
    private readonly PasswordHasher hasher;
    public AuthService()
    {
        repo = new Repository.UserRepository();
        hasher = new PasswordHasher();
    }

    public async Task<Result> RegisterUser(UserAccountDto userDto)
    {
        // Ensure that the email does not already exist
        if (!string.IsNullOrEmpty(userDto.Email) && await repo.IsEmailExistAsync(userDto.Email))
        {
            return Result.Failure("email is exist");
        }

        // Ensure that the Phone Number does not already exist
        if (!string.IsNullOrEmpty(userDto.PhoneNumber) && await repo.IsPhoneExistAsync(userDto.PhoneNumber))
        {
            return Result.Failure("Phone Number is exist");
        }

        // Send data to UserRepository for create new user acount
        return await repo.CreateAsync(userDto);
    }

    public async Task<Result<UserLoginResponseDto>> UserLoginByEmailAsync(string email, string password)
    {
        // Check if email and password are provided
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Result<UserLoginResponseDto>.Failure("Email or password is missing.");
        }

        // Find user account by email
        var userAccount = await repo.FindUserByEmailAsync(email);
        if (userAccount != null)
        {
            return await Login(userAccount, password);
        }

        // Return failure result if email or password is incorrect
        return Result<UserLoginResponseDto>.Failure("Invalid email or password.");
    }

    public async Task<Result<UserLoginResponseDto>> UserLoginByPhoneAsync(string phone, string password)
    {
        // Check if phone and password are provided
        if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
        {
            return Result<UserLoginResponseDto>.Failure("phone or password is missing.");
        }

        // Find user account by phone
        var userAccount = await repo.FindUserByPhoneNumberAsync(phone);
        if (userAccount != null)
        {
            return await Login(userAccount, password);
        }

        // Return failure result if phone or password is incorrect
        return Result<UserLoginResponseDto>.Failure("Invalid phone or password.");
    }

    private async Task<Result<UserLoginResponseDto>> Login(UserAccount userAccount, string password)
    {
        // Check if acount is active
        if (userAccount.IsActive)
        {
            // if user login in any device you shoud logout after relogin
            if (userAccount.Access)
                return Result<UserLoginResponseDto>.Failure("You are login in other device please logout from it");

            // Verify the provided password with the stored password hash
            var isPasswordCorrect = await hasher.VerifyPasswordAsync(password, userAccount.PasswordHash);
            if (isPasswordCorrect)
            {
                // Map user account to UserLoginResponseDto
                var loginRes = new UserLoginResponseDto
                {
                    Id = userAccount.Id,
                    Email = userAccount.Email,
                    EmailConfirmed = userAccount.EmailConfirmed,
                    FullName = userAccount.FullName,
                    PharmcyName = userAccount.PharmcyName,
                    PhoneNumber = userAccount.PhoneNumber
                };

                // update access in userAcount 
                await repo.SetUserAccountIsAccessAsync(userAccount);

                // Return successful result with user login response and welcome message
                return Result<UserLoginResponseDto>.Success(loginRes, $"Welcome {userAccount.FullName}");
            }
            else
            {
                // Increment AccessFailedCount if the password is incorrect
                await repo.IncrementAccessFailedCountAsync(userAccount);
                return Result<UserLoginResponseDto>.Failure("Invalid password.");
            }
        }
        else
        {
            return Result<UserLoginResponseDto>.Failure("your acount is not active");
        }
    }

    public async Task<Result> Logout(int id)
    {
        return await repo.UpdateUserAccountIsLockoutAsync(id);
    }



    //public async Task<Result<UserLoginResponseDto>> UserLoginByEmailAsync(string email, string password)
    //{
    //    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    //    {
    //        var userAcount = await repo.FindUserByEmailAsync(email);
    //        if (userAcount is not null && await hasher.VerifyPasswordAsync(password, userAcount.PasswordHash))
    //        {
    //            var loginRes = new UserLoginResponseDto
    //            {
    //                Id = userAcount.Id,
    //                Email = userAcount.Email,
    //                EmailConfirmed = userAcount.EmailConfirmed,
    //                FullName = userAcount.FullName,
    //                PharmcyName = userAcount.PharmcyName,
    //                PhoneNumber = userAcount.PhoneNumber
    //            };
    //            return Result<UserLoginResponseDto>.Success(loginRes,$"Welcom{userAcount.FullName}");
    //        }
    //    }
    //    return Result<UserLoginResponseDto>.Failure("email or password is .....");
    //}
}
