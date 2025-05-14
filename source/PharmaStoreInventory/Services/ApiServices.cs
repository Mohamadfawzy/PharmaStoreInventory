using CommunityToolkit.Mvvm.Messaging;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using DataAccess.Services;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.Models.User;

namespace PharmaStoreInventory.Services;

public static class ApiServices
{
    #region Dashboard
    public static async Task<bool> TestConnection()
    {
        return await RequestProvider.GetBooleanValueAsync(AppValues.LocalBaseURI + "/connection");
    }
    
    public static async Task<object> TestConnection(string url)
    {
        return await RequestProvider.GetSingleAsync<object>(url);
    }

    public static async Task<StatisticsModel?> GetProductCountsAsync(int storeId)
    {
        return await RequestProvider.GetSingleAsync<StatisticsModel>(AppValues.LocalBaseURI + $"/statistic-all?storeId={storeId}");
    }

    public static async Task<InventoryHistory?> GetLatestInventoryHistoryAsync(int storeId)
    {
        return await RequestProvider.GetSingleAsync<InventoryHistory>(AppValues.LocalBaseURI + $"/Commons/latest_inventory_history?sotreId={storeId}");
    }

    public static async Task<List<Stores>?> GetAllStores()
    {
        var (content, error) = await RequestProvider.GetAllAsync<Stores>(AppValues.LocalBaseURI + $"/Commons/stores");
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result<InventoryHistory>?> StartNewInventoryAsync(StartNewInventoryHistoryDto model)
    {
        var (content, error) =
            await RequestProvider.PostSingleAsync<Result<InventoryHistory>, StartNewInventoryHistoryDto>(AppValues.LocalBaseURI + $"/Commons/start_new_inventory", model);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }
    #endregion

    #region Product
    public static async Task<List<ProductDto>?> GetAllProducts(ProductQParam qParam)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "query.StoreId",qParam.StoreId},
                { "query.IsGroup",qParam.IsGroup.ToString()},
                { "query.QuantityBiggerThanZero",qParam.QuantityBiggerThanZero.ToString()},
                { "query.Text",qParam.Text},
                { "query.HasExpire",qParam.HasExpire.ToString()},
                { "query.PageSize",qParam.PageSize.ToString()},
                { "query.OrderBy",qParam.OrderBy.ToString()},
            };

        var url = AppValues.LocalBaseURI + "/Products/all" + await BuildQueryString(queryParams);


        var (content, error) = await RequestProvider.GetAllAsync<ProductDto>(url);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<List<ProductDetailsModel>?> GetProductDetails(bool hasOnlyQuantity, string productCode, int storeId)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "hasOnlyQuantity",hasOnlyQuantity.ToString()},
                { "barcode",productCode},
                { "storeId",storeId.ToString() }
            };

        var url = AppValues.LocalBaseURI + "/Products/product-details" + await BuildQueryString(queryParams);

        var (content, error) = await RequestProvider.GetAllAsync<ProductDetailsModel>(url);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result?> UpdateInventoryStatus(bool allOneTime, string status, int productId, int expiryBatchID)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "allOneTime",allOneTime.ToString()},
                { "status",status},
                { "productId",productId.ToString() },
                { "expiryBatchID",expiryBatchID.ToString() }
            };

        var url = AppValues.LocalBaseURI + "/Products/edit-inventory-status" + await BuildQueryString(queryParams);

        return await RequestProvider.PutByQueryParamsAsync<Result>(url);
    }

    public static async Task<Result?> UpdateQuantity(UpdateProductQuantityDto model)
    {
        var url = AppValues.LocalBaseURI + "/Products/edit-quantity";
        return await RequestProvider.PutAsync<Result, UpdateProductQuantityDto>(url, model);
    }

    public static async Task<Result?> CopyProductAmount(CopyProductAmoutDto model)
    {
        var url = AppValues.LocalBaseURI + "/Products/copy";
        var (content, error) = await RequestProvider.PostSingleAsync<Result, CopyProductAmoutDto>(url, model);
        if (error != null)
        {
            //WeakReferenceMessenger.Default.Send(new PickingViewNotification(error));
        }
        return content;
    }
    #endregion

    #region Branche
    public static async Task<Result?> AddBranche(BranchModel model)
    {
        var uri = $"{AppValues.HostBaseURI}/Branche/add";
        var (content, error) = await RequestProvider.PostSingleAsync<Result, BranchModel>(uri, model);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result?> DeleteBranche(Guid branchId)
    {
        var uri = $"{AppValues.HostBaseURI}/Branche/delete?brancheId={branchId}";
        return await RequestProvider.DeleteAsync<Result>(uri);
    }

    public static async Task<List<BranchModel>?> GetAllBranches(int userId)
    {
        var (content, error) = await RequestProvider.GetAllAsync<BranchModel>($"{AppValues.HostBaseURI}/Branche/all?userId={userId}");
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new CreateBranchViewNotification(error));
        }
        return content;
    }
    #endregion

    #region Employee
    public static async Task<(ConnectionErrorCode err, string? message)> ApiEmployeeLogin
        (BranchModel branch, string accountUsername,  string accountPassword )
    {
        try
        {
            var baseUrl = await Configuration.ConfigureBaseUrl(branch.IpAddress, branch.Port);

            var emp = new LoginBothSidesDTO
            {
                AccountUsername = accountUsername,
                AccountPassword = accountPassword,
                EmpPassword = branch.Password,
                EmpUsername = branch.Username,
                SecretKey = AppConstants.SecretKey
            };

            var url = baseUrl + "/Auth/local-login";
            var (response, error) = await RequestProvider.PostSingleAsync<UserLoginResponseDTO, LoginBothSidesDTO>(url, emp);


            // status 1 Unable to connect to server
            if (response == null)
            {
                return (ConnectionErrorCode.Fail, string.Empty);
            }
            // status 2 Server connection IsSuccess
            if (response != null)
            {
                if (response.IsSuccess)
                {
                    if (response.User != null)
                    {
                        AppPreferences.LocalDbUserId = response.User.Id;
                        AppPreferences.Token = "Bearer " + response.AccessToken;
                        return (ConnectionErrorCode.Success, string.Empty);
                    }
                    return (ConnectionErrorCode.Fail, "data or token in null");
                }
                // status 3 Server connection IsSuccess, but username or password is incorrect
                return (ConnectionErrorCode.UsernameOrPass, response.Message);
            }
            return (ConnectionErrorCode.Fail, "response or result or auth in null");
        }
        catch (Exception ex)
        {
            return (ConnectionErrorCode.Exception, ex.Message); ;
        }
    }
    #endregion

    #region User
    public static async Task<Result?> RegisterUserAsync(UserRegisterDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/registration";
        var (content, error) = await RequestProvider.PostSingleAsync<Result, UserRegisterDto>(url, model);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result?> IsEmailExistAsync(string email)
    {
        var url = AppValues.HostBaseURI + $"/userAuth/is-email-exist/{email}";
        return await RequestProvider.GetSingleAsync<Result>(url);
    }

    public static async Task<Result?> CanRegisterEmailOrPhoneAsync(string email, string phone)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "email",email},
                { "phone",phone}
            };
        var url = AppValues.HostBaseURI + "/userAuth/email-phone-exist" + await BuildQueryString(queryParams);
        return await RequestProvider.GetSingleAsync<Result>(url);
    }

    public static async Task<Result?> IsUserActiveAsync(int userId)
    {

        var url = AppValues.HostBaseURI + "/userAuth/user_status?userId=" + userId;
        return await RequestProvider.GetSingleAsync<Result>(url);
    }

    public static async Task<Result<UserLoginResponseDto>?> UserLoginByEmailAsync(UserLoginRequestDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/login-email";
        var (content, error) = await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result<UserLoginResponseDto>?> UserLoginByPhoneAsync(UserLoginRequestDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/login-phone";
        var (content, error) = await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
        if (error != null)
        {
            WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        }
        return content;
    }

    public static async Task<Result?> EditUserDataAsync(UserEditDataDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/edit_user_info";
        return await RequestProvider.PutAsync<Result, UserEditDataDto>(url, model);
    }
    
    public static async Task<Result?> UserChangePasswordAsync(ChangePasswordRequest model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/change-password";
        return await RequestProvider.PutAsync<Result, ChangePasswordRequest>(url, model);
    }
    
    public static async Task<Result?> ResetPasswordAsync(ResetPasswordRequest model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/reset-forgotten-password";
        return await RequestProvider.PutAsync<Result, ResetPasswordRequest>(url, model);
    }
    #endregion

    #region Version
    public static async Task<SystemVersions?> GetCurrentSystemVersionAsync(string url)
    {
        return await RequestProvider.GetSingleAsync<SystemVersions>(url + $"/system-version");
    }
    public static async Task<PharmaVersion?> GetCurrentPharmaVersionAsync()
    {
        return await RequestProvider.GetSingleAsync<PharmaVersion>(AppValues.HostBaseURI + $"/pharma-version");
    }
    #endregion

    #region Email
    public static async Task<Result?> PostEmailVerificationCode(EmailRequestModel model)
    {
        var url = AppValues.HostBaseURI + "/mail/save";
        var (content, error) = await RequestProvider.PostSingleAsync<Result, EmailRequestModel>(url, model);
        return content;
    }

    public static async Task<Result?> PostAndSendEmail(EmailRequestModel emailModel)
    {
        var url = AppValues.HostBaseURI + "/mail/send";
        var (content, error) = await RequestProvider.PostSingleAsync<Result, EmailRequestModel>(url, emailModel);
        return content;
    }
    
    //public static async Task<Result?> PostAndSendEmail(EmailRequestModel emailModel)
    //{
    //    MailingService mailingService = new();
    //    try
    //    {
    //        //1 save code in database
    //        var saveCodeResponse = await PostEmailVerificationCode(emailModel);

    //        //2 send email
    //        if (saveCodeResponse != null && saveCodeResponse.IsSuccess)
    //        {
    //            var res = await mailingService.SendVerificationCodeAsync(emailModel);
    //            if (res != null && res.IsSuccess)
    //            {
    //                return Result.Success();
    //            }
    //        }
    //        return Result.Failure("حدثت مشكلة ولم يتم ارسال الكود");

    //    }
    //    catch (Exception ex)
    //    {
    //        return Result.Failure(ErrorCode.ExceptionError, ex.Message);
    //    }
    //}

    public static async Task<Result?> IsVerificationCodeValidAsync(Guid evcId, string code)
    {
        var url = AppValues.HostBaseURI + $"/mail/find";
        var model = new EmailVerificationParameters(evcId, code);
        return await RequestProvider.PutAsync<Result, EmailVerificationParameters>(url,model);
    }
    #endregion

    // BuildQueryString
    private static async Task<string> BuildQueryString(Dictionary<string, string?> queryParams)
    {
        try
        {
            if (queryParams.Count == 0)
                return "";

            var query = new System.Text.StringBuilder("?");
            foreach (var param in queryParams)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    query.Append(Uri.EscapeDataString(param.Key));
                    query.Append('=');
                    query.Append(Uri.EscapeDataString(param.Value));
                    query.Append('&');
                }
            }
            return query.ToString().TrimEnd('&');
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackBar($"{nameof(RequestProvider)}:{nameof(BuildQueryString)}, Message:{ex.Message}");
            return string.Empty;
        }
    }
}