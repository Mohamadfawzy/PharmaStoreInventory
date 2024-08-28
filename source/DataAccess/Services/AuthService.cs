using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.Helper;
using DataAccess.Repository;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Services;

public class AuthService(UserRepository _repo)
{
    //private readonly Repository.UserRepository repo = new();
    private readonly PasswordHasher hasher = new();
    private readonly MailingService mailService = new();
    private readonly UserRepository repo = _repo;


    #region Admin Section
    public async Task<Result> AdminLoginByEmailAsync(string email, string password)
    {
        bool isPasswordCorrect;
        // Check if email and password are provided
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return Result.Failure("Email or password is missing.");
        }

        try
        {
            // All user account by email
            var userAccount = await repo.FindUserByEmailAsync(email.ToLower()).ConfigureAwait(false);
            if (userAccount == null)
            {
                return Result.Failure("admin Invalid email or password.");
            }

            // If admin is the first time he will log in
            if (userAccount.PasswordHash == password)
            {
                var result = await repo.UpdatePasswordByEmailAsync(email, password).ConfigureAwait(false);
                isPasswordCorrect = result.IsSuccess;

            }
            else
            {
                // Verify the provided password with the stored password hash
                isPasswordCorrect = await hasher.VerifyPasswordAsync(password, userAccount.PasswordHash).ConfigureAwait(false);
            }

            if (isPasswordCorrect)
            {
                // Return successful result with user login response and welcome message
                return Result.Success($"Welcome {userAccount.FullName}");
            }
            else
            {
                // Increment AccessFailedCount if the password is incorrect
                await repo.IncrementAccessFailedCountAsync(userAccount).ConfigureAwait(false);
                return Result.Failure("Invalid password.");
            }
        }
        catch (Exception ex)
        {
            return Result.Exception(ex.Message);
        }
    }

    public async Task<Result> ChangeUserStatus(int userId, bool status)
    {
        return await repo.UpdateActivationUserAccountAsync(userId, status);
    }

    public async Task<List<UserInfoDto>> GetAllUsersAsync(FilterUsersQParam qParam)
    {
        return await repo.ReadAllUsers(qParam);
    }
    public async Task<Result> AdminConfirmsUserEmail(int userId)
    {
        if (userId == 0)
            return Result.Failure(ErrorCode.InvalidIdentifier, "Can not found user equal 0");

        return await repo.AdminConfirmsUserEmail(userId);
    }
    #endregion


    public async Task<Result> RegisterUserAcync(UserRegisterDto userDto)
    {
        // Ensure that the email does not already exist
        if (!string.IsNullOrEmpty(userDto.Email) && await repo.IsEmailExistAsync(userDto.Email))
        {
            return Result.Failure("The provided email is already in use by an existing user. Each user must have a unique email.");
        }

        // Ensure that the Phone Number does not already exist
        if (!string.IsNullOrEmpty(userDto.PhoneNumber) && await repo.IsPhoneExistAsync(userDto.PhoneNumber))
        {
            return Result.Failure("The provided phone number is already in use by an existing user. Each user must have a unique phone number.");
        }

        // Send data to UserRepository for create new user acount
        return await repo.CreateAsync(userDto);
    }


    public async Task<Result> IsEmailOrPhoneExistAsync(string email, string phone)
    {
        // Ensure that the email does not already exist
        if (string.IsNullOrEmpty(email) || await repo.IsEmailExistAsync(email))
        {
            return Result.Failure(ErrorCode.EmailAlreadyExists, "Email already exists");
        }
        // Check if phone and password are provided
        if (string.IsNullOrEmpty(phone) || await repo.IsPhoneExistAsync(phone))
        {
            return Result.Failure(ErrorCode.PhoneNumberAlreadyExists, "Phone number already exists");
        }

        return Result.Success();
    }


    /// <summary>
    /// Authenticates a user based on their email and password.
    /// </summary>
    /// <param name="deviceId">The device ID from which the user is logging in.</param>
    /// <returns>A <see cref="Result{UserLoginResponseDto}"/> containing the user's login information if authentication is successful, or an error message if authentication fails.</returns>
    public async Task<Result<UserLoginResponseDto>> UserLoginByEmailAsync(UserLoginRequestDto dto)
    {
        // Check if email and password are provided
        if (string.IsNullOrEmpty(dto.EmailOrPhone) || string.IsNullOrEmpty(dto.Password))
        {
            return Result<UserLoginResponseDto>.Failure("Email or password is missing.");
        }
        try
        {
            // All user account by email
            var userAccount = await repo.FindUserByEmailAsync(dto.EmailOrPhone).ConfigureAwait(false);
            if (userAccount is null)
            {
                // Return failure result if email or password is incorrect 
                return Result<UserLoginResponseDto>.Failure("Invalid email or password.");
            }

            return await Login(userAccount, dto.Password, dto.DviceId, dto.IsNewDevice).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result<UserLoginResponseDto>.Failure($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Authenticates a user based on their phone number and password.
    /// </summary>
    /// <param name="deviceId">The device ID from which the user is logging in.</param>
    /// <returns>A <see cref="Result{UserLoginResponseDto}"/> containing the user's login information if authentication is successful, or an error message if authentication fails.</returns>
    public async Task<Result<UserLoginResponseDto>> UserLoginByPhoneAsync(UserLoginRequestDto dto)
    {
        // Check if phone and password are provided
        if (string.IsNullOrEmpty(dto.EmailOrPhone) || string.IsNullOrEmpty(dto.Password))
        {
            return Result<UserLoginResponseDto>.Failure("Phone or password is missing.");
        }
        try
        {
            // All user account by phone
            var userAccount = await repo.FindUserByPhoneNumberAsync(dto.EmailOrPhone).ConfigureAwait(false);
            if (userAccount is null)
            {
                // Return failure result if phone or password is incorrect
                return Result<UserLoginResponseDto>.Failure("Invalid phone or password.");
            }
            return await Login(userAccount, dto.Password, dto.DviceId, dto.IsNewDevice).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result<UserLoginResponseDto>.Failure($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Locks out a user account by updating the access status and set date.now in LockoutEnd.
    /// </summary>
    /// <param name="id">The ID of the user account to be locked out.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the lockout operation.</returns>
    public async Task<Result> LogoutUserAsync(int id)
    {
        // Update the lockout status of the user account
        return await repo.UpdateLogoutUserAsync(id).ConfigureAwait(false);
    }

    /// <summary>
    /// Changes the password for a user account.
    /// </summary>
    /// <param name="dto">The request data transfer object containing user ID, new password, and confirmation password.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the password reset operation.</returns>
    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest dto)
    {
        // Check if the new password matches the confirmation password
        if (dto.NewPassword != dto.ConfirmNewPassword)
        {
            return Result.Failure("NewPassword does not match ConfirmNewPassword");
        }

        // All the user by ID
        var user = await repo.FindUserById(dto.UserId).ConfigureAwait(false);

        // Check if the user exists
        if (user == null)
        {
            // Return failure if the user is not found
            return Result.Failure($"There is no existing user record corresponding to the provided identifier. User not found with ID: {dto.UserId}");
        }

        // Check if the user is logged in
        if (!user.IsLoggedIn)
        {
            // Return failure if the user is not logged in
            return Result.Failure(ErrorCode.UserLogOut, "User is not logged in.");
        }

        var passwordIsCorrect = await hasher.VerifyPasswordAsync(dto.CurrentPassword, user.PasswordHash).ConfigureAwait(false);
        if (passwordIsCorrect == false)
        {
            return Result.Failure(ErrorCode.PasswordIsIncorrect, "Password Is Wrong.");

        }
        // Update the password and return the result
        return await repo.UpdatePasswordByUserIdAsync(dto.UserId, dto.NewPassword).ConfigureAwait(false);
    }

    public async Task<Result> SaveAndSendVerificationCodeAsync(string email)
    {
        // Check if email and password are provided
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure("Email is missing.");
        }
        try
        {
            // All user account by email
            var userAccount = await repo.FindUserByEmailAsync(email).ConfigureAwait(false);
            if (userAccount != null)
            {
                var code = Common.GenerateVerificationCode();
                var vCodeExpirationTime = DateTimeOffset.UtcNow.AddMinutes(Helper.Strings.VerificationCodeMinutesExpires);
                await mailService.SendVerificationCodeAsync(email, code, userAccount.FullName).ConfigureAwait(false);
                return await repo.UpdateVerificationCode(userAccount.Id, code, vCodeExpirationTime).ConfigureAwait(false);
            }

        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred: {ex.Message}");
        }

        // Return failure result if email or password is incorrect
        return Result.Failure("Invalid email");
    }

    public async Task<Result> ResetForgottenPasswordAsync(string email, string verificationCode, string newPassword)
    {
        // Check if email and password are provided
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(verificationCode) || string.IsNullOrEmpty(newPassword))
        {
            return Result.Failure("Email or verificationCode or newPassword is missing.");
        }
        try
        {
            // All user account by email
            var userAccount = await repo.FindUserByEmailAsync(email).ConfigureAwait(false);
            if (userAccount != null && userAccount.VerificationCode == verificationCode)
            {
                return await repo.UpdatePasswordByEmailAsync(email, newPassword).ConfigureAwait(false);
            }

        }
        catch (Exception ex)
        {
            return Result.Failure($"An error occurred: {ex.Message}");
        }

        // Return failure result if email or password is incorrect
        return Result.Failure("Invalid email or password.");

        // Update the password and return the result
    }

    public async Task<Result> EditUserInfoAsync(UserEditNameDto dto)
    {
        return await repo.UpdateUserInfoAsync(dto).ConfigureAwait(false);
    }



    // ### PrivateMethods ###########################################################################################################################################################################
    /// <summary>
    /// Verifies the provided password and logs in the user if the credentials are correct.
    /// </summary>
    /// <param name="userAccount">The user's account information.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="deviceId">The device ID from which the user is logging in.</param>
    /// <returns>A <see cref="Result{UserLoginResponseDto}"/> containing the user's login information if authentication is successful, or an error message if authentication fails.</returns>
    private async Task<Result<UserLoginResponseDto>> Login(UserAccount userAccount, string password, string deviceId, bool isNewDevice)
    {
        // Check if account is active
        if (!userAccount.IsActive)
        {
            return Result<UserLoginResponseDto>.Failure(ErrorCode.UserNotActive, "Your account is not active.");
        }

        // Check if user is logged in another device
        if (userAccount.IsLoggedIn && (!string.IsNullOrEmpty(userAccount.DeviceID) && userAccount.DeviceID != deviceId) && !isNewDevice)
        {
            return Result<UserLoginResponseDto>.Failure("You are logged in on another device, please log out from it.");
        }

        // Verify the provided password with the stored password hash
        var isPasswordCorrect = await hasher.VerifyPasswordAsync(password, userAccount.PasswordHash).ConfigureAwait(false);
        if (isPasswordCorrect)
        {

            // if The password will be correct. and user wanted login from new device you shoud admin activate this new device
            userAccount.IsActive = !isNewDevice;
            userAccount.DeviceID = deviceId;
            // Map user account to UserLoginResponseDto
            var loginRes = new UserLoginResponseDto
            {
                Id = userAccount.Id,
                Email = userAccount.Email,
                EmailConfirmed = userAccount.EmailConfirmed,
                FullName = userAccount.FullName,
                PharmcyName = userAccount.PharmacyName,
                PhoneNumber = userAccount.PhoneNumber,
                LockoutEnd = userAccount.LockoutEnd = DateTimeOffset.UtcNow.AddDays(7),
                IsActive = userAccount.IsActive,
            };

            // Update access in userAccount
            await repo.SetUserAccountIsAccessAsync(userAccount).ConfigureAwait(false);

            // Return successful result with user login response and welcome message
            return Result<UserLoginResponseDto>.Success(loginRes, $"Welcome {userAccount.FullName}");
        }
        else
        {
            // Increment AccessFailedCount if the password is incorrect
            await repo.IncrementAccessFailedCountAsync(userAccount).ConfigureAwait(false);
            return Result<UserLoginResponseDto>.Failure("Invalid password.");
        }
    }

}
