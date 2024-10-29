using AdminApp.AppData;
using CommunityToolkit.Mvvm.Messaging;
using DataTransferObjects.UserDTOs;
using Shared.MAUI.Builders;
using Shared.MAUI.Services;
using Shared.Models;
using Shared.RequestFeatures;

namespace AdminApp.Services.ApiServices;

internal class UserApiService
{
    public static async Task<Result<UserLoginResponseDto>?> UserLoginByEmailAsync(UserLoginRequestDto model)
    {
        var url = StaticData.BaseUrl + "/userAuth/login-email";
        var requestResult = await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
        //if (error != null)
        //{
        //    //WeakReferenceMessenger.Default.Send(new NotificationMessage(error));
        //}
        return requestResult.Content;
    }
    public static async Task<Result<UserLoginResponseDto>?> UserLoginByPhoneAsync(UserLoginRequestDto model)
    {
        var url = StaticData.BaseUrl + "/userAuth/login-phone";
        var requestResult = await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
        return requestResult.Content;
    }


    public static async Task<Result<UserLoginResponseDto>?> AdminLoginByEmailAsync(LoginDto model)
    {
        var url = StaticData.BaseUrl + "/admin/login";
        var providerResult = await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>, LoginDto>(url, model);
        return providerResult.Content;
    }

    public static async Task<List<UserInfoDto>?> GetUsersAsync(UserParameters query)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "query.IsActive",query.IsActive.ToString()},
                { "query.EmailConfirmed",query.EmailConfirmed.ToString()},
                { "query.OrderBy",query.OrderBy.ToString()},
                { "query.Role",query.Role.ToString()},
                { "query.Page",query.Page.ToString()},
                { "query.PageSize",query.PageSize.ToString()}
            };

        var url = StaticData.BaseUrl + "/admin/all-users" + QueryBuilder.BuildQueryString(queryParams);

        var providerResult= await RequestProvider.GetAllAsync<UserInfoDto>(url);
        return providerResult.Content;
    }

    public static async Task<Result?> ChangeUserStatus(int userId)
    {
        var queryParams = new Dictionary<string, string?>
            {
                { "userId",userId.ToString()},
                { "status","true"}
            };

        var url = StaticData.BaseUrl + "/admin/activate-user-account" + QueryBuilder.BuildQueryString(queryParams);

        var providerResult =  await RequestProvider.PutByQueryParamsAsync<Result>(url);
        return providerResult.Content;
    }
}
