using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Dtos.UserDtos;
using DataAccess.Entities;
using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Services;

public static class ApiServices
{
    #region Dashboard
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
        return await RequestProvider.GetAllAsync<Stores>(AppValues.LocalBaseURI + $"/Commons/stores");
    }
    public static async Task<Result<InventoryHistory>?> StartNewInventoryAsync(StartNewInventoryHistoryDto model)
    {
        return await RequestProvider.PostSingleAsync<Result<InventoryHistory>, StartNewInventoryHistoryDto>(AppValues.LocalBaseURI + $"/Commons/start_new_inventory", model);
    }
    #endregion


    #region Product
    public static async Task<List<ProductDto>?> GetAllProducts(ProductQParam qParam)
    {
        var queryParams = new Dictionary<string, string>
            {
                { "query.StoreId",qParam.StoreId},
                { "query.IsGroup",qParam.IsGroup.ToString()},
                { "query.QuantityBiggerThanZero",qParam.QuantityBiggerThanZero.ToString()},
                { "query.Text",qParam.Text},
                { "query.OrderBy",qParam.OrderBy.ToString()},
            };

        var url = AppValues.LocalBaseURI + "/Products/all" + await BuildQueryString(queryParams);

        return await RequestProvider.GetAllAsync<ProductDto>(url);
    }

    public static async Task<List<ProductDetailsDto>?> GetProductDetails(bool hasOnlyQuantity, string productCode, int storeId)
    {
        var queryParams = new Dictionary<string, string>
            {
                { "hasOnlyQuantity",hasOnlyQuantity.ToString()},
                { "barcode",productCode},
                { "storeId",storeId.ToString() }
            };

        var url = AppValues.LocalBaseURI + "/Products/product-details" + await BuildQueryString(queryParams);

        return await RequestProvider.GetAllAsync<ProductDetailsDto>(url);
    }

    public static async Task<Result?> UpdateInventoryStatus(bool allOneTime, string status, int productId, int expiryBatchID)
    {
        var queryParams = new Dictionary<string, string>
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
    #endregion product

    public static async Task<Result<EmployeeDto?>?> EmpLogin(EmpLogin model)
    {
        var url = AppValues.LocalBaseURI + "/Employee/login";
        return await RequestProvider.PostSingleAsync<Result<EmployeeDto?>, EmpLogin>(url, model);
    }


    #region User
    public static async Task<Result?> RegisterUserAcync(UserRegisterDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/registration";
        return await RequestProvider.PostSingleAsync<Result, UserRegisterDto>(url, model);
    }

    public static async Task<Result?> IsEmailOrPhoneExistAsync(string email, string phone)
    {
        var queryParams = new Dictionary<string, string>
            {
                { "email",email},
                { "phone",phone}
            };
        var url = AppValues.HostBaseURI + "/userAuth/email-phone-exist" + await BuildQueryString(queryParams);
        return await RequestProvider.GetSingleAsync<Result>(url);
    }

    public static async Task<Result<UserLoginResponseDto>?> UserLoginByEmailAsync(UserLoginRequestDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/login-email";
        return await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
    }
    public static async Task<Result<UserLoginResponseDto>?> UserLoginByPhoneAsync(UserLoginRequestDto model)
    {
        var url = AppValues.HostBaseURI + "/userAuth/login-email";
        return await RequestProvider.PostSingleAsync<Result<UserLoginResponseDto>?, UserLoginRequestDto>(url, model);
    }
    #endregion
























    // BuildQueryString
    private static async Task<string> BuildQueryString(Dictionary<string, string> queryParams)
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
            await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(BuildQueryString)}, Message:{ex.Message}");
            return string.Empty;
        }
    }
}
