using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Repository;

//public class Temp
//{
//    public decimal? ProductId { get; set; }
//    public string? ProductNameAr { get; set; }
//    public string? ProductNameEn { get; set; }

//    public decimal? ProductUnit1 { get; set; }
//    public decimal? ProductUnit13 { get; set; }
//    public string? CompanyNameAr { get; set; }
//}
public class ProductAmountRepo(AppDb context)
{
    private readonly AppDb context = context;
    const string whereDeletedActiveStatement = " WHERE p.Deleted = 1 AND p.Active = 1 ";
    public async Task<List<ProductDto>> GetAllProducts(ProductQParam qParam)
    {
        string selectClause = await BringSelectAllProductsIncludeJoinQuery(qParam.IsGroup, qParam.PageSize);
        string whereQuantity = qParam.QuantityBiggerThanZero ? " AND pa.Amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(qParam.StoreId) ? "" : $" AND pa.Store_id = {qParam.StoreId} ";
        string whereSiteId = string.IsNullOrEmpty(qParam.SiteId) ? "" : $" AND p.site_id = {qParam.SiteId} ";
        string whereHasExpire = string.IsNullOrEmpty(qParam.HasExpire) ? " " : $" AND p.product_has_expire = {qParam.HasExpire} ";
        string whereOrderBy = $" ORDER BY {BringOrderByQuery(qParam.OrderBy)}";
        string whereSearchText = await BringSearchQuery(qParam.Text);
        //string offset = await BringPaginationQuery(qParam.Page);
        string sql = @$"{selectClause} 
                        {whereDeletedActiveStatement}
                        {whereHasExpire}
                        {whereQuantity}
                        {whereSearchText}
                        {whereStoreId}
                        {whereSiteId}
                        {whereOrderBy}";
        //{offset}
        return await context.Database
            .SqlQueryRaw<ProductDto>(sql)
            .ToListAsync();
    }

    public async Task<List<ProductDetailsDto>?> GetProductDetails(bool hasOnlyQuantity, string productCode, int storeId)
    {
        var barcode = ExtractBarcode(productCode);
        if (string.IsNullOrEmpty(barcode))
            return default;

        string whereQuantity = hasOnlyQuantity ? " AND pa.Amount > 0 " : " ";
        string sql = @$" 
                SELECT
	                pa.Pa_id AS [ID],
                    pa.Product_id AS [ProductId],
                    pa.Store_id AS [StoreId],
                    pa.Counter_id AS [ExpiryGroupID],
	                pa.Product_update AS [IsInventoried],
                    pa.Exp_date AS [ExpDate],
                    CAST(pa.Amount AS DECIMAL(18, 2)) AS [Quantity],
                    CAST(pa.Sell_price AS DECIMAL(18, 2)) AS [SellPrice],
                    pa.product_update AS [ProductUpdate],
                    p.product_name_ar AS [ProductNameAr],
                    p.product_name_en AS [ProductNameEn],
                    p.product_unit1 AS [ProductUnit1],
                    p.product_has_expire AS [ProductHasExpire],
                    vendor.vendor_name_ar AS [VendorNameAr]

                FROM products as p
                INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
                INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
                LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id
                    {whereDeletedActiveStatement} 
                    AND pa.Store_id = @StoreId
                    {whereQuantity} 
                    AND (
                    p.[product_code]       = @Barcode OR
                    p.[product_int_code]   = @Barcode) 
                    order by pa.counter_id desc ";
        //          order by pa.Exp_date desc 

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


        /*
          OR
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
                    p.[product_int_code14] = @Barcode
         */
    }

    public async Task<ProductDetailsDto?> GetProductById(int id)
    {
        var pro = await context.Product_Amount
            .FirstOrDefaultAsync(x => x.Pa_id == id);
        return (ProductDetailsDto?)pro;
    }

    public async Task<Result> UpdateInventoryStatus(bool allOneTime, string status, int productId, int expiryBatchID)
    {
        var query = context.Product_Amount
            .Where(x => x.Product_id == productId);
        if (!allOneTime)
            query = query.Where(u => u.Counter_id == expiryBatchID);

        var row = await query.ExecuteUpdateAsync(s => s.SetProperty(e => e.Product_update, e => status));

        if (row > 0)
            return Result.Success();
        return Result.Failure();
    }

    public async Task<Result> CopyProductAmout(CopyProductAmoutDto model)
    {
        try
        {
            var exists = await context.Product_Amount
                .AnyAsync(x => x.Exp_date == model.ExpDate && x.Product_id == model.ProductId);

            if (exists)
            {
                return Result.Failure(ErrorCode.ItemIsExist, $"This {model.ExpDate} in already Exist");
            }

            var expiryBatchID = context.Product_Amount.Count(x => x.Product_id == model.ProductId);

            var product = context.Product_Amount
                .AsNoTracking()
                .FirstOrDefault(x => x.Pa_id == model.Id);

            if (product == null)
                return Result.Failure(ErrorCode.NotFoundById, "this product is not founded");

            var param = new ProductUpdateAndChangeQParam()
            {
                Notice = "إضافة كمية جديدة بتاريخ جديد",
                OldExp_date = product.Exp_date,
                Old_amount = 0,
                Product_unit1 = model.ProductUnit1,
                EmpId = model.EmpId,
            };


            product.Pa_id = 0;
            product.Amount = model.Quantity;
            product.Counter_id = ++expiryBatchID;
            product.Exp_date = model.ExpDate;
            product.Insert_uid = model.EmpId;
            product.Update_uid = model.EmpId;
            product.Product_update = "1";

            context.Product_Amount.Add(product);

            await InsertProductAmountUpdatesAsync(product, param).ConfigureAwait(false);
            await InsertProductAmountChangesAsync(product, param).ConfigureAwait(false);

            var effectedRow = await context.SaveChangesAsync();
            if (effectedRow > 0)
            {
                return Result.Success($"{effectedRow} row effected");
            };
            return Result.Failure(ErrorCode.OperationFailed, "Can not add this product, effectedRow = 0");
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorCode.ExceptionError, ex.Message);
        }
    }

    public async Task<Result> UpdateQuantity(UpdateProductQuantityDto dto)
    {
        try
        {
            var product = await context.Product_Amount
                .FirstOrDefaultAsync(x => x.Pa_id == dto.Id);

            if (product == null)
                return Result.Failure(ErrorCode.NotFoundById);


            var param = new ProductUpdateAndChangeQParam()
            {
                Notice = dto.Notes,
                OldExp_date = product.Exp_date,
                Old_amount = product.Amount,
                Product_unit1 = dto.ProductUnit1,
                EmpId = dto.EmpId,
            };
            product.Amount = (dto.NewQuantity - dto.OldQuantity) + product.Amount;
            product.Exp_date = dto.ExpDate;
            product.Insert_uid = dto.EmpId;
            product.Update_uid = dto.EmpId;
            product.Product_update = "1";
            product.Update_date = DateTime.Now;
            product.Product_update_date = DateTime.Now;

            await InsertProductAmountUpdatesAsync(product, param).ConfigureAwait(false);
            await InsertProductAmountChangesAsync(product, param).ConfigureAwait(false);

            var effectedRow = await context.SaveChangesAsync();
            if (effectedRow > 0)
            {
                return Result.Success($"product: {dto.ProductId} is updated successfully");
            };
            return Result.Failure("not updated");
        }
        catch (Exception ex)
        {
            return Result.Failure("An error occurred while checking the expiration date." + ex.Message + ex.InnerException?.Message);
        }
    }

    public async Task InsertProductAmountUpdatesAsync(Product_Amount pa, ProductUpdateAndChangeQParam param)
    {
        var pau = new ProductAmountUpdate()
        {
            Product_id = pa.Product_id,
            Store_id = pa.Store_id,
            Counter_id = pa.Counter_id,
            Insert_uid = param.EmpId,

            Old_amount = param.Old_amount,
            Old_exp_date = param.OldExp_date,

            New_amount = pa.Amount,
            New_exp_date = pa.Exp_date,
            Notes = param.Notice,

            Buy_price = pa.Buy_price,
            Store_date = pa.Insert_date,
            Sell_price = pa.Sell_price,
            Tax_price = pa.Tax_price,
            Vendor_id = pa.Vendor_id,
            Product_unit = param.Product_unit1,
            Insert_date = DateTime.Now,
        };
        var status = await context.ProductAmountUpdates.AddAsync(pau);
    }

    public async Task InsertProductAmountChangesAsync(Product_Amount pa, ProductUpdateAndChangeQParam parm)
    {
        var pac = new ProductAmountChange()
        {
            Product_id = pa.Product_id,
            Store_id = pa.Store_id,
            Counter_id = pa.Counter_id,
            New_amount = pa.Amount,
            Buy_price = pa.Buy_price,
            Sell_price = pa.Sell_price,
            Tax_price = pa.Tax_price,
            Vendor_id = pa.Vendor_id,
            Exp_date = pa.Exp_date,
            Form_type = 26,
            In_type = "0",

            Old_amount = parm.Old_amount,
            Insert_uid = parm.EmpId,
            Form_notice = parm.Notice,
            Insert_date = DateTime.Now,
        };
        await context.ProductAmountChanges.AddAsync(pac);
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
    public async Task<Result> CanAddExpirationDate(int id, int productId, DateTime? date)
    {
        try
        {
            //if (date == null)
            //    return Result.Failure("date Time is null");

            // Check if a product with the same expiration date and product ID exists, but with a different ID
            var exists = await context.Product_Amount
                .AnyAsync(x => x.Exp_date == date && x.Product_id == productId && x.Pa_id != id);

            // If a conflicting record is found, return failure
            if (exists)
                return Result.Failure("An expiration date already exists for this product with the same date.");

            // If no conflicting record is found, return success
            return Result.Success("The expiration date can be added.");
        }
        catch (Exception ex)
        {
            // Return a generic failure message
            return Result.Failure("An error occurred while checking the expiration date." + ex.Message);
        }
    }

    #region Statistics
    public async Task<StatisticsModel?> GetProductCountsAsync(int storeId)
    {
        var currentDate = DateTime.Now;
        var threeMonthsLater = currentDate.AddMonths(3);

        var result = await context.Product_Amount
            .AsNoTracking()
            .Where(p => p.Amount > 0 && p.Store_id == storeId)
            .GroupBy(p => 1)
            .Select(g => new StatisticsModel
            {
                TotalProducts = g.Count(),
                ExpiredProducts = g.Count(p => p.Exp_date < currentDate),
                WillExpireIn3Months = g.Count(p => p.Exp_date > currentDate && p.Exp_date < threeMonthsLater),
                InventoryedProducts = g.Count(p => p.Product_update == "1")
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return result;
    }

    public async Task<int> GetCountAllProductsAsync(int storeId)
    {
        // Use AsNoTracking to improve performance if no tracking is needed
        return await context.Product_Amount
            .AsNoTracking()
            .Where(p => p.Amount > 0 && p.Store_id == storeId)
            .CountAsync()
            .ConfigureAwait(false); // Ensure proper task scheduling and avoid deadlocks.
    }

    public async Task<int> GetCountAllExpiredProducts(int storeId)
    {
        var totalProduct = await context.Product_Amount
            .AsNoTracking()
            .Where(p => p.Amount > 0 && p.Exp_date < DateTime.Now && p.Store_id == storeId)
            .Select(p => p.Product_id)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllProductsWillExpireAfter3Months(int storeId)
    {
        var currentDate = DateTime.Now;
        var threeMonthsLater = currentDate.AddMonths(3);

        var totalProduct = await context.Product_Amount
            .AsNoTracking()
            .Where(p => p.Amount > 0
                        && p.Exp_date > currentDate
                        && p.Exp_date < threeMonthsLater
                        && p.Store_id == storeId)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllIsInventoryed(int storeId)
    {
        var totalProduct = await context.Product_Amount
            .AsNoTracking()
            .Where(p => p.Amount > 0 && p.Product_update == "1" && p.Store_id == storeId)
            .CountAsync();
        return totalProduct;
    }
    #endregion

    // The code that's violating the rule is on this line.
    #region Methods for creating queries
#pragma warning disable CA1822
    private Task<string> BringSelectAllProductsIncludeJoinQuery(bool isGroup, int pageSize = 50)
    {
        const string columnsStatement = @"  p.product_code AS [ProductCode],
                                        p.product_int_code AS [InternationalCode],
                                        ISNULL(NULLIF(p.product_name_en, ''),
                                        p.product_name_ar) AS [Name],
                                        CAST(pa.Amount AS DECIMAL(18, 2)) AS [Quantity],
                                        CAST(pa.Sell_price AS DECIMAL(18, 2)) AS [SalePrice],
                                        pa.Store_id AS [StoreId] ,
                                        p.product_has_expire AS [HasExpire] ";
        if (isGroup)
        {
            var sql = @$" SELECT top ({pageSize}) {columnsStatement}
                          FROM (SELECT Store_id, Product_id, SUM(Amount) AS Amount, Sell_price 
                                 FROM Product_Amount GROUP BY Product_id,Sell_price,Store_id) AS pa 
                          INNER JOIN Products p ON pa.Product_id = p.Product_id ";
            return Task.FromResult(sql);
        }
        else
        {
            var sql = @$" SELECT top ({pageSize}) {columnsStatement}
                        FROM Products p
                        INNER JOIN Product_Amount pa ON p.Product_id = pa.Product_id ";
            return Task.FromResult(sql);
        }
    }

    private string BringOrderByQuery(ProductsOrderBy orderBy)
    {
        string clause = orderBy switch
        {
            ProductsOrderBy.Non => "(SELECT 1)",
            ProductsOrderBy.ProductId => "pa.product_id",
            ProductsOrderBy.BiggestPrice => "pa.Sell_price desc",
            ProductsOrderBy.LowestPrice => "pa.Sell_price",
            ProductsOrderBy.Name => "p.product_name_en",
            ProductsOrderBy.MaxQuantity => "pa.Amount desc ",
            ProductsOrderBy.MinQuantity => "pa.Amount",
            _ => "(SELECT 1)",
        };
        return $" {clause} ";
    }

    private Task<string> BringPaginationQuery(int page, int size = 30)
    {
        var num = (page < 2) ? 0 : (page - 1) * size;
        return Task.FromResult($"OFFSET {num} ROWS FETCH NEXT {size} ROWS ONLY ");
    }

    private Task<string> BringSearchQuery(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            if (Validator.IsNumeric(searchText))
            {
                return Task.FromResult($" AND (p.product_code ='{searchText}' or p.product_int_code = '{searchText}') ");
            }
            else
            {
                return Task.FromResult($" AND (p.product_fast_code = '{searchText}'or p.product_name_en  LIKE '{searchText}%' or p.product_name_ar  LIKE '{searchText}%') ");
            }
        }
        return Task.FromResult("");
    }

    private string ExtractBarcode(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            string[] words = input.Split('-', '$');
            return words[0];
        }
        return string.Empty;
    }

    //string GetBarcodeQuery()
    //{

    //    return "";
    //}
#pragma warning restore CA1822
    #endregion


    /*
    #region Trash

        //public async Task<List<ProductDetailsDto>?> GetProductDetailsByProcedure(short hasOnlyQuantity, string productCode, int storeId)
    //{
    //    string? barcode = ExtractBarcode(productCode);
    //    if (string.IsNullOrEmpty(barcode))
    //        return default;

    //    var parameters = new[]
    //    {
    //        new SqlParameter("@StoreId", storeId),
    //        new SqlParameter("@Barcode", barcode),
    //        new SqlParameter("@HasOnlyQuantity", hasOnlyQuantity)
    //    };

    //    var results = await context.Database
    //        .SqlQueryRaw<ProductDetailsDto>("EXEC GetProductDetails @StoreId, @Barcode, @HasOnlyQuantity", parameters)
    //        .ToListAsync();
    //    return results;
    //}


    public async Task<List<ProductDetailsDto>?> GetProductDetailsLinq(bool hasOnlyQuantity, string productCode, decimal? storeId)
    {
        var barcode = ExtractBarcode(productCode);
        if (string.IsNullOrEmpty(barcode))
            return default;
        var query = from p in context.Products
                    join pAmount in context.Product_Amount on p.product_id equals pAmount.Product_id
                    join company in context.Companys on p.Company_id equals company.Company_id
                    join productUnit in context.Product_units on p.product_unit1 equals productUnit.unit_id
                    join vendor in context.Vendor on pAmount.Vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where p.Deleted == "1" && p.Active == "1"
                    where (hasOnlyQuantity == false || pAmount.Amount > 0)
                    where (!storeId.HasValue || pAmount.Store_id == storeId)
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
                    orderby pAmount.Exp_date descending
                    select new ProductDetailsDto
                    {
                        Id = (int)pAmount.Pa_id,
                        ProductId = pAmount.Product_id,
                        StoreId = pAmount.Store_id,
                        ExpiryGroupID = pAmount.Counter_id,
                        ExpDate = pAmount.Exp_date,
                        //VendorId = pAmount.Vendor_id,
                        Quantity = pAmount.Amount,
                        SellPrice = pAmount.Sell_price,
                        IsInventoried = pAmount.Product_update,
                        ProductNameAr = p.product_name_ar,
                        ProductNameEn = p.product_name_en,
                        ProductUnit1 = p.product_unit1,
                        ProductUnit13 = p.product_unit1_3,
                        VendorNameAr = vendor.vendor_name_ar,
                        CompanyNameAr = company.Co_name_ar
                    };

        var result = await query.ToListAsync();
        return result;
    }
    
    public async Task<List<ProductDetailsDto>?> GetSingleProductByBarcode(bool hasOnlyQuantity, string barcode, decimal? storeId)
    {
        //var barcode = ExtractBarcode(productCode);
        //if (string.IsNullOrEmpty(barcode))
        //    return default;
       var sql = $@"declare @barcode varchar(50) ='22'
SELECT
	pa.Pa_id AS [Id],
    pa.Product_id AS [ProductId],
    pa.Store_id AS [StoreId],
    pa.Counter_id AS [ExpiryGroupID],
	pa.Product_update AS [IsInventoried],
    pa.Exp_date AS [ExpDate],
    pa.Amount AS [Quantity],
    pa.Sell_price AS [SellPrice],
    p.product_name_ar AS [ProductNameAr],
    p.product_name_en AS [ProductNameEn],
    p.product_unit1 AS [ProductUnit1],
    p.product_unit1_3 AS [ProductUnit13],
    vendor.vendor_name_ar AS [VendorNameAr],
    companys.Co_name_ar AS [CompanyNameAr]

FROM products as p
INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
INNER JOIN companys ON p.Company_id = companys.Company_id
INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id

WHERE 
 pa.Store_id=1
 AND p.Deleted=1
 AND p.Active=1
 AND pa.Amount > -1
    AND (
           p.product_code = @barcode
        OR p.product_int_code = @barcode
        OR p.product_int_code1 = @barcode
        OR p.product_int_code2 = @barcode
        OR p.product_int_code3 = @barcode)
 order by pa.Exp_date desc;
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
    
    public async Task<List<ProductDetailsDto>?> GetSingleProAmountByProId(bool hasOnlyQuantity, string productCode, decimal? storeId)
    {
        var query = from pAmount in context.Product_Amount
                    join vendor in context.Vendor on pAmount.Vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where (hasOnlyQuantity == false || pAmount.Amount > 0)
                    where (!storeId.HasValue || pAmount.Store_id == storeId)
                    where pAmount.Product_id == 22
                    orderby pAmount.Exp_date descending
                    select new ProductDetailsDto
                    {
                        Id = (int)pAmount.Pa_id,
                        ProductId = pAmount.Product_id,
                        StoreId = pAmount.Store_id,
                        ExpiryGroupID = pAmount.Counter_id,
                        ExpDate = pAmount.Exp_date,
                        Quantity = pAmount.Amount,
                        SellPrice = pAmount.Sell_price,
                        IsInventoried = pAmount.Product_update,
                        VendorNameAr = vendor.vendor_name_ar,
                    };

        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<ProductDetailsDto>?> GetProductDetails(bool hasOnlyQuantity, string productCode, int storeId)
    {
        var barcode = ExtractBarcode(productCode);
        if (string.IsNullOrEmpty(barcode))
            return default;

        string whereQuantity = hasOnlyQuantity ? " AND pa.Amount > 0 " : " ";
        string sql = @$" 
                SELECT
	                pa.Pa_id AS [ID],
                    pa.Product_id AS [ProductId],
                    pa.Store_id AS [StoreId],
                    pa.Counter_id AS [ExpiryGroupID],
	                pa.Product_update AS [IsInventoried],
                    pa.Exp_date AS [ExpDate],
                    pa.Amount AS [Quantity],
                    pa.Sell_price AS [SellPrice],
                    pa.product_update AS [ProductUpdate],
                    p.product_name_ar AS [ProductNameAr],
                    p.product_name_en AS [ProductNameEn],
                    p.product_unit1 AS [ProductUnit1],
                    p.product_unit1_3 AS [ProductUnit13],
                    vendor.vendor_name_ar AS [VendorNameAr],
                    companys.Co_name_ar AS [CompanyNameAr]

                FROM products as p
                INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
                INNER JOIN companys ON p.Company_id = companys.Company_id
                INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
                LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id
                    {whereDeletedActiveStatement} 
                    AND pa.Store_id = @StoreId
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
                    order by pa.Exp_date desc ";

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
    
    public async Task<List<ProductDetailsDto>?> GetProductDetailsFormADO(bool hasOnlyQuantity, string productCode, int storeId)
    {
        var barcode = ExtractBarcode(productCode);
        if (string.IsNullOrEmpty(barcode))
            return default;

        string whereQuantity = hasOnlyQuantity ? " AND pa.Amount > 0 " : " ";
        string connectionString = "Data Source=192.168.1.103,1433\\MSSQLSERVER01; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
        var conn = new SqlConnection(connectionString);

        string sql = @$" 
                SELECT
	                pa.Pa_id AS [ID],
                    pa.Product_id AS [ProductId],
                    pa.Amount AS [Quantity],
                    pa.Sell_price AS [SellPrice],
                    p.product_name_ar AS [ProductNameAr],
                    p.product_name_en AS [ProductNameEn]

                FROM products as p
                INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
                INNER JOIN companys ON p.Company_id = companys.Company_id
                INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
                LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id
                    WHERE p.Deleted = 1 AND p.Active = 1  
                    AND pa.Store_id = 1
                    
                    AND (
                    p.[product_code]       = @Barcode OR
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
                    order by pa.Exp_date desc ";

        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Barcode", "22");

        cmd.CommandType = System.Data.CommandType.Text;
        await conn.OpenAsync();

        SqlDataReader reader = cmd.ExecuteReader();

        ProductDetailsDto product;
        var list = new List<ProductDetailsDto>();
        while (reader.Read())
        {
            product = new ProductDetailsDto()
            {
                Id = reader.GetDecimal("Id"),
                ProductId = reader.GetDecimal("ProductId"),
                Quantity = reader.GetDecimal("Quantity"),
                SellPrice = reader.GetDecimal("SellPrice"),
                ProductNameAr = reader.GetString("ProductNameAr"),
                ProductNameEn = reader.GetString("ProductNameEn"),
            };
            list.Add(product);
        }
        conn.Close();
        return list;
    }
    #endregion

    */

}