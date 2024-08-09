using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace DataAccess.Repository;

public class Temp{
    public decimal? ProductId { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }

    public decimal? ProductUnit1 { get; set; }
    public decimal? ProductUnit13 { get; set; }
    public string? CompanyNameAr { get; set; }
}
public class ProductAmountRepo
{
    private readonly AppDb context;
    const string whereDeletedActiveStatement = " WHERE p.deleted = 1 AND p.active = 1 ";
    const string columnsStatement = @" p.product_code AS [ProductCode], p.product_int_code AS [InternationalCode], ISNULL(NULLIF(p.product_name_en, ''), p.product_name_ar) AS [Name], pa.sell_price AS [SalePrice], pa.amount AS [Quantity], pa.store_id AS [StoreId] ";

    public ProductAmountRepo()
    {
        context = new();
    }

    public async Task<List<ProductDto>> GetAllProducts(ProductQParam qParam)
    {
        string selectClause = BringSelectAllProductsIncludeJoinQuery(qParam.IsGroup);
        string whereQuantity = qParam.QuantityBiggerThanZero ? " AND pa.amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(qParam.StoreId) ? "" : $" AND pa.store_id = {qParam.StoreId} ";
        string whereSiteId = string.IsNullOrEmpty(qParam.SiteId) ? "" : $" AND products.site_id = {qParam.SiteId} ";
        string whereOrderBy = $" ORDER BY {BringOrderByQuery((ProductsOrderBy)qParam.OrderBy)}";
        string whereSearchText = BringSearchQuery(qParam.Text);
        string offset = BringPaginationQuery(qParam.Page);
        string sql = @$"{selectClause} 
                        {whereDeletedActiveStatement}
                        {whereQuantity}
                        {whereSearchText}
                        {whereStoreId}
                        {whereSiteId}
                        {whereOrderBy}
                        {offset}";
        return await context.Database
            .SqlQueryRaw<ProductDto>(sql)
            .ToListAsync();
    }
    public async Task<List<ProductDetailsDto>?> GetProductDetailsByProcedure(short hasOnlyQuantity, string barcodeParam, int storeId)
    {
        string? barcode = ExtractBarcode(barcodeParam);
        if (string.IsNullOrEmpty(barcode))
            return default;
        
        var parameters = new[]
        {
            new SqlParameter("@StoreId", storeId),
            new SqlParameter("@Barcode", barcodeParam),
            new SqlParameter("@HasOnlyQuantity", hasOnlyQuantity)
        };

        var results = await context.Database
            .SqlQueryRaw<ProductDetailsDto>("EXEC GetProductDetails @StoreId, @Barcode, @HasOnlyQuantity", parameters)
            .ToListAsync();
        return results;


    }





    public async Task<List<ProductDetailsDto>?> GetProductDetailsLinq(bool hasOnlyQuantity, string barcodeParam, decimal? storeId)
    {
        var barcode = ExtractBarcode(barcodeParam);
        if (string.IsNullOrEmpty(barcode))
            return default;
        var query = from p in context.Products
                    join pAmount in context.Product_Amount on p.product_id equals pAmount.product_id
                    join company in context.Companys on p.company_id equals company.company_id
                    join productUnit in context.Product_units on p.product_unit1 equals productUnit.unit_id
                    join vendor in context.Vendor on pAmount.vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where p.deleted == "1" && p.active == "1"
                    where (hasOnlyQuantity == false || pAmount.amount > 0)
                    where (!storeId.HasValue || pAmount.store_id == storeId)
                    where (p.product_code == barcode
                        || p.product_fast_code == barcode
                        || p.product_int_code == barcode
                        || p.product_int_code1 == barcode
                        || p.product_int_code2 == barcode
                        || p.product_int_code3 == barcode
                        || p.product_int_code4 == barcode
                        || p.product_int_code5 == barcode
                        || p.product_int_code6 == barcode
                        || p.product_int_code7 == barcode
                        || p.product_int_code8 == barcode
                        || p.product_int_code9 == barcode
                        || p.product_int_code10 == barcode
                        || p.product_int_code11 == barcode
                        || p.product_int_code12 == barcode
                        || p.product_int_code13 == barcode
                        || p.product_int_code14 == barcode)
                    orderby pAmount.exp_date descending
                    select new ProductDetailsDto
                    {
                        Id = (int)pAmount.pa_id,
                        ProductId = pAmount.product_id,
                        StoreId = pAmount.store_id,
                        ExpiryGroupID = pAmount.counter_id,
                        ExpDate = pAmount.exp_date,
                        //VendorId = pAmount.vendor_id,
                        Quantity = pAmount.amount,
                        SellPrice = pAmount.sell_price,
                        IsInventoried = pAmount.Product_update,
                        ProductNameAr = p.product_name_ar,
                        ProductNameEn = p.product_name_en,
                        ProductUnit1 = p.product_unit1,
                        ProductUnit13 = p.product_unit1_3,
                        VendorNameAr = vendor.vendor_name_ar,
                        CompanyNameAr = company.co_name_ar
                    };

        var result = await query.ToListAsync();
        return result;
    }
    
    public async Task<List<ProductDetailsDto>?> GetSingleProductByBarcode(bool hasOnlyQuantity, string barcode, decimal? storeId)
    {
        //var barcode = ExtractBarcode(barcodeParam);
        //if (string.IsNullOrEmpty(barcode))
        //    return default;

       var sql = $@"declare @barcode varchar(50) ='22'
SELECT
	pa.pa_id AS [Id],
    pa.product_id AS [ProductId],
    pa.store_id AS [StoreId],
    pa.counter_id AS [ExpiryGroupID],
	pa.Product_update AS [IsInventoried],
    pa.exp_date AS [ExpDate],
    pa.amount AS [Quantity],
    pa.sell_price AS [SellPrice],
    p.product_name_ar AS [ProductNameAr],
    p.product_name_en AS [ProductNameEn],
    p.product_unit1 AS [ProductUnit1],
    p.product_unit1_3 AS [ProductUnit13],
    vendor.vendor_name_ar AS [VendorNameAr],
    companys.co_name_ar AS [CompanyNameAr]

FROM products as p
INNER JOIN Product_Amount as pa ON p.product_id = pa.product_id 
INNER JOIN companys ON p.company_id = companys.company_id
INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
LEFT OUTER JOIN vendor ON pa.vendor_id = vendor.vendor_id

WHERE 
 pa.store_id=1
 AND p.deleted=1
 AND p.active=1
 AND pa.amount > -1
    AND (
           p.product_code = @barcode
        OR p.product_int_code = @barcode
        OR p.product_int_code1 = @barcode
        OR p.product_int_code2 = @barcode
        OR p.product_int_code3 = @barcode)
 order by pa.exp_date desc;
";

        // Define parameters to avoid SQL injection and type issues
        var parameters = new[]
        {
            new SqlParameter("@StoreId", storeId),
            new SqlParameter("@Barcode", barcode)
        };

        var list = await context.Database
            .SqlQueryRaw<ProductDetailsDto>(sql)
            .ToListAsync();

        List<ProductDetailsDto> newList = new();
        foreach (var item in list)
        {
            ProductDetailsDto ll = new ProductDetailsDto() 
            { ProductId = item.ProductId, 
                ProductNameAr = item.ProductNameAr, 
                ProductNameEn = item.ProductNameEn,
                CompanyNameAr = item.CompanyNameAr,
                ProductUnit1 = item.ProductUnit1,
                ProductUnit13 = item.ProductUnit13,
                
            };
            newList.Add(ll);
        };
        return list;
    }
    
    public async Task<List<ProductDetailsDto>?> GetSingleProAmountByProId(bool hasOnlyQuantity, string barcodeParam, decimal? storeId)
    {
        var query = from pAmount in context.Product_Amount
                    join vendor in context.Vendor on pAmount.vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where (hasOnlyQuantity == false || pAmount.amount > 0)
                    where (!storeId.HasValue || pAmount.store_id == storeId)
                    where pAmount.product_id == 22
                    orderby pAmount.exp_date descending
                    select new ProductDetailsDto
                    {
                        Id = (int)pAmount.pa_id,
                        ProductId = pAmount.product_id,
                        StoreId = pAmount.store_id,
                        ExpiryGroupID = pAmount.counter_id,
                        ExpDate = pAmount.exp_date,
                        Quantity = pAmount.amount,
                        SellPrice = pAmount.sell_price,
                        IsInventoried = pAmount.Product_update,
                        VendorNameAr = vendor.vendor_name_ar,
                    };

        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<ProductDetailsDto>?> GetProductDetails(bool hasOnlyQuantity, string barcodeParam, int storeId)
    {
        string barcode = ExtractBarcode(barcodeParam);
        if (string.IsNullOrEmpty(barcode))
            return default;

        string whereQuantity = hasOnlyQuantity ? " AND pa.amount > 0 " : " ";
        string sql = @$" 
                SELECT
	                pa.pa_id AS [ID],
                    pa.product_id AS [ProductId],
                    pa.store_id AS [StoreId],
                    pa.counter_id AS [ExpiryGroupID],
	                pa.Product_update AS [IsInventoried],
                    pa.exp_date AS [ExpDate],
                    pa.amount AS [Quantity],
                    pa.sell_price AS [SellPrice],
                    pa.product_update AS [ProductUpdate],
                    p.product_name_ar AS [ProductNameAr],
                    p.product_name_en AS [ProductNameEn],
                    p.product_unit1 AS [ProductUnit1],
                    p.product_unit1_3 AS [ProductUnit13],
                    vendor.vendor_name_ar AS [VendorNameAr],
                    companys.co_name_ar AS [CompanyNameAr]

                FROM products as p
                INNER JOIN Product_Amount as pa ON p.product_id = pa.product_id 
                INNER JOIN companys ON p.company_id = companys.company_id
                INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
                LEFT OUTER JOIN vendor ON pa.vendor_id = vendor.vendor_id
                    {whereDeletedActiveStatement} 
                    AND pa.store_id = @StoreId
                    {whereQuantity} 
                    AND (
                    p.[product_code]       = @Barcode OR
                    p.[product_int_code]   = @Barcode OR
                    p.[product_int_code1]  = @Barcode OR
                    p.[product_int_code2]  = @Barcode OR
                    p.[product_int_code3]  = @Barcode OR
                    p.[product_int_code4]  = @Barcode OR
                    p.[product_int_code5]  = @Barcode OR
                    p.[product_int_code6]  = @Barcode OR
                    p.[product_int_code7]  = @Barcode OR
                    p.[product_int_code8]  = @Barcode OR
                    p.[product_int_code9]  = @Barcode OR
                    p.[product_int_code10] = @Barcode OR
                    p.[product_int_code11] = @Barcode OR
                    p.[product_int_code12] = @Barcode OR
                    p.[product_int_code13] = @Barcode OR
                    p.[product_int_code14] = @Barcode) 
                    order by pa.exp_date desc ";

        // Define parameters to avoid SQL injection and type issues
        var parameters = new[]
        {
            new SqlParameter("@StoreId", storeId),
            new SqlParameter("@Barcode", barcode)
        };

        var list = await context.Database
            .SqlQueryRaw<ProductDetailsDto>(sql, parameters)
            .ToListAsync();
        return list;
    }
    
    




    public async Task<ProductDetailsDto?> GetProductById(int id)
    {
        var pro = await context.Product_Amount
            .FirstOrDefaultAsync(x => x.pa_id == id);
        return (ProductDetailsDto?)pro;
    }

    public async Task<bool> UpdateInventoryStatus(bool allOneTime, string status, int? productId, int? expiryBatchID)
    {
        var query = context.Product_Amount
            .Where(x => x.product_id == productId);
        if (!allOneTime)
            query = query.Where(u => u.counter_id == expiryBatchID);

        var row = await query.ExecuteUpdateAsync(s => s.SetProperty(e => e.Product_update, e => status));
        return (row > 0);
    }

    public async Task<bool> InsertProductAmout(Product_Amount item)
    {
        //var item = context.Product_Amount.AsNoTracking().ProductsOrderBy(x => x.counter_id).LastOrDefault(x => x.product_id == 22);
        Product_Amount? insertedRow;
        if (item != null)
        {
            item.counter_id = ++item.counter_id;
            item.pa_id = 0;
            /*
            var pro = new Product_Amount()
            {
                amount = item.amount,
                counter_id = ++item.counter_id,
                product_id = item.product_id,
                batch_num = item.batch_num,
                buy_price = item.buy_price,
                exp_date = item.exp_date,
                insert_date = item.insert_date,
                insert_uid = item.insert_uid,
                Product_update = item.Product_update,
                Product_update_date = item.Product_update_date,
                sell_price = item.sell_price,
                store_id = item.store_id,
                tax_price = item.tax_price,
                update_date = item.update_date,
                update_uid = item.update_uid,
                vendor_id = item.vendor_id
            }; */
            insertedRow = context.Add(item).Entity;
            var effectedRow = await context.SaveChangesAsync();
            if (effectedRow > 0)
                return true;
        }
        return false;
    }

    public async Task<Result> CopyProductAmout(int id, int productId, decimal quantity, DateTime? expDate)
    {
        var expiryBatchID = context.Product_Amount.Count(x => x.product_id == productId);

        var product = context.Product_Amount
            .AsNoTracking()
            .FirstOrDefault(x => x.pa_id == id);

        if (product == null)
            return Result.Failure();

        product.pa_id = 0;
        product.amount = quantity;
        product.counter_id = ++expiryBatchID;
        product.exp_date = expDate;
        product.insert_uid = Helper.Constants.UserId;
        product.update_uid = Helper.Constants.UserId;
        product.Product_update = "1";

        context.Product_Amount.Add(product);
        var effectedRow = await context.SaveChangesAsync();
        if (effectedRow > 0)
        {
            return Result.Success($"{effectedRow} row effected");
        }

        return Result.Failure();
    }

    public async Task<Result> UpdateQuantity(int id, decimal oldQuantity, decimal newQuantity, DateTime? expDate)
    {
        var product = await context.Product_Amount
            .FirstOrDefaultAsync(x => x.pa_id == id);

        if (product == null)
            return Result.Failure(ErrorCode.NotFoundById);

        product.amount = (newQuantity - oldQuantity) + product.amount;
        product.exp_date = expDate ?? expDate;
        product.insert_uid = Helper.Constants.UserId;
        product.update_uid = Helper.Constants.UserId;
        product.Product_update = "1";
        product.update_date = DateTime.Now;
        product.Product_update_date = DateTime.Now;

        var effectedRow = await context.SaveChangesAsync();
        if (effectedRow > 0)
        {
            return Result.Success();
        };

        return Result.Failure("not updated");
    }

    /// <summary>
    /// Checks whether an expiration date can be added for a specified product.
    /// Verifies if there is already an existing record with the same expiration date 
    /// and product ID but a different ID.
    /// </summary>
    /// <param name="id">The ID of the record to exclude from the check.</param>
    /// <param name="productId">The ID of the product to check against.</param>
    /// <param name="date">The expiration date to be added.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the expiration date can be added.
    /// Returns a failure result if a conflicting record exists, otherwise returns a success result.
    /// </returns>
    public async Task<Result> CanAddExpirationDate(int id, int productId, DateTime date)
    {
        try
        {
            // Check if a product with the same expiration date and product ID exists, but with a different ID
            var exists = await context.Product_Amount
                .AnyAsync(x => x.exp_date == date && x.product_id == productId && x.pa_id != id);

            // If a conflicting record is found, return failure
            if (exists)
                return Result.Failure("An expiration date already exists for this product with the same date.");

            // If no conflicting record is found, return success
            return Result.Success("The expiration date can be added.");
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // Log.Error(ex, "An error occurred while checking the expiration date.");

            // Return a generic failure message
            return Result.Failure("An error occurred while checking the expiration date." + ex.Message);
        }
    }

    //public async Task<Result> IsExpirationDateExisting(int id, int productId, DateTime date)
    //{
    //    try
    //    {
    //        var result = context.Product_Amount
    //            .Any(x => x.exp_date == date && x.product_id == productId);
    //        if (result)
    //            return new ContextResponse(result, "exp_date is Existing");
    //        return new ContextResponse(false, "exp_date is not Existing");
    //    }
    //    catch
    //    {
    //        return new ContextResponse(false, "catch error"); ;
    //    }
    //}

    #region Statistics
    public async Task<int> GetCountAllProducts(int storeId)
    {
        var totalProduct = await context.Product_Amount
            .Where(p => p.amount > 0 && p.store_id == storeId)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllExpiredProducts(int storeId)
    {
        var totalProduct = await context.Product_Amount
            .Where(p => p.amount > 0 && p.exp_date < DateTime.Now && p.store_id == storeId)
            .Select(p => p.product_id)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllProductsWillExpireAfter3Months(int storeId)
    {
        var currentDate = DateTime.Now;
        var threeMonthsLater = currentDate.AddMonths(3);

        var totalProduct = await context.Product_Amount
            .Where(p => p.amount > 0
                        && p.exp_date > currentDate
                        && p.exp_date < threeMonthsLater
                        && p.store_id == storeId)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllIsInventoryed(int storeId)
    {
        var totalProduct = await context.Product_Amount
            .Where(p => p.amount > 0 && p.Product_update == "1" && p.store_id == storeId)
            .CountAsync();
        return totalProduct;
    }
    #endregion

    #region Methods for creating queries
    string BringSelectAllProductsIncludeJoinQuery(bool isGroup)
    {
        if (isGroup)
        {
            return @$" SELECT {columnsStatement}
                    FROM (SELECT store_id, product_id, SUM(amount) AS amount, sell_price 
                        FROM Product_Amount GROUP BY product_id,sell_price,store_id) 
                        AS pa 
                        INNER JOIN Products p ON pa.product_id = p.product_id ";
        }
        else
        {
            return @$" SELECT {columnsStatement}
                        FROM Products p
                        INNER JOIN Product_Amount pa ON p.product_id = pa.product_id ";
        }

    }

    string BringOrderByQuery(ProductsOrderBy orderBy)
    {
        string clause;
        switch (orderBy)
        {
            case ProductsOrderBy.Non:
                clause = "(SELECT 1)";
                break;
            case ProductsOrderBy.ProductId:
                clause = "p.product_code";
                break;
            case ProductsOrderBy.BiggestPrice:
                clause = "p.sell_price desc";
                break;
            case ProductsOrderBy.LowestPrice:
                clause = "p.sell_price";
                break;
            default:
                clause = "(SELECT 1)";
                break;
        }
        return $" {clause} ";
    }

    string BringPaginationQuery(int page, int size = 30)
    {
        var num = (page < 2) ? 0 : (page - 1) * size;
        return $"OFFSET {num} ROWS FETCH NEXT {size} ROWS ONLY ";
    }

    string BringSearchQuery(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            //var containsEnglishLetters = Regex.IsMatch(searchText, @"[a-zA-Z]");
            var isNumeric = Regex.IsMatch(searchText, @"^\d+$");

            if (isNumeric)
            {
                return $" AND (p.product_code ='{searchText}' or p.product_int_code = '{searchText}') ";
            }
            else
            {
                return $" AND (p.product_fast_code = '{searchText}'or p.product_name_en  LIKE '%{searchText}%' or p.product_name_ar  LIKE '%{searchText}%') ";
            }
        }
        return string.Empty;
    }

    string? ExtractBarcode(string input)
    {
        string[] words = input.Split('-', '$');
        return words[0];
    }

    string GetBarcodeQuery()
    {

        return "";
    }
    #endregion
}