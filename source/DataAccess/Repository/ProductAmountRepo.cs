using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace DataAccess.Repository;

public class Temp
{
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
    const string whereDeletedActiveStatement = " WHERE p.Deleted = 1 AND p.active = 1 ";
    const string columnsStatement = @" p.product_code AS [ProductCode], p.product_int_code AS [InternationalCode], ISNULL(NULLIF(p.product_name_en, ''), p.product_name_ar) AS [Name], pa.Sell_price AS [SalePrice], pa.Amount AS [Quantity], pa.Store_id AS [StoreId] ";

    public ProductAmountRepo()
    {
        context = new();
    }

    public async Task<List<ProductDto>> GetAllProducts(ProductQParam qParam)
    {
        string selectClause = BringSelectAllProductsIncludeJoinQuery(qParam.IsGroup);
        string whereQuantity = qParam.QuantityBiggerThanZero ? " AND pa.Amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(qParam.StoreId) ? "" : $" AND pa.Store_id = {qParam.StoreId} ";
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
            new SqlParameter("@Barcode", barcode),
            new SqlParameter("@HasOnlyQuantity", hasOnlyQuantity)
        };

        var results = await context.Database
            .SqlQueryRaw<ProductDetailsDto>("EXEC GetProductDetails @StoreId, @Barcode, @HasOnlyQuantity", parameters)
            .ToListAsync();
        return results;


    }





    /*
    #region Trash
    public async Task<List<ProductDetailsDto>?> GetProductDetailsLinq(bool hasOnlyQuantity, string barcodeParam, decimal? storeId)
    {
        var barcode = ExtractBarcode(barcodeParam);
        if (string.IsNullOrEmpty(barcode))
            return default;
        var query = from p in context.Products
                    join pAmount in context.Product_Amount on p.product_id equals pAmount.Product_id
                    join company in context.Companys on p.company_id equals company.company_id
                    join productUnit in context.Product_units on p.product_unit1 equals productUnit.unit_id
                    join vendor in context.Vendor on pAmount.Vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where p.deleted == "1" && p.active == "1"
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
    companys.co_name_ar AS [CompanyNameAr]

FROM products as p
INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
INNER JOIN companys ON p.company_id = companys.company_id
INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id

WHERE 
 pa.Store_id=1
 AND p.Deleted=1
 AND p.active=1
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
    
    public async Task<List<ProductDetailsDto>?> GetSingleProAmountByProId(bool hasOnlyQuantity, string barcodeParam, decimal? storeId)
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

    public async Task<List<ProductDetailsDto>?> GetProductDetails(bool hasOnlyQuantity, string barcodeParam, int storeId)
    {
        var barcode = ExtractBarcode(barcodeParam);
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
                    companys.co_name_ar AS [CompanyNameAr]

                FROM products as p
                INNER JOIN Product_Amount as pa ON p.Product_id = pa.Product_id 
                INNER JOIN companys ON p.company_id = companys.company_id
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
    
    public async Task<List<ProductDetailsDto>?> GetProductDetailsFormADO(bool hasOnlyQuantity, string barcodeParam, int storeId)
    {
        var barcode = ExtractBarcode(barcodeParam);
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
                INNER JOIN companys ON p.company_id = companys.company_id
                INNER JOIN product_units ON p.product_unit1 = product_units.unit_id
                LEFT OUTER JOIN vendor ON pa.Vendor_id = vendor.Vendor_id
                    WHERE p.Deleted = 1 AND p.active = 1  
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




    public async Task<ProductDetailsDto?> GetProductById(int id)
    {
        var pro = await context.Product_Amount
            .FirstOrDefaultAsync(x => x.Pa_id == id);
        return (ProductDetailsDto?)pro;
    }

    public async Task<bool> UpdateInventoryStatus(bool allOneTime, string status, int? productId, int? expiryBatchID)
    {
        var query = context.Product_Amount
            .Where(x => x.Product_id == productId);
        if (!allOneTime)
            query = query.Where(u => u.Counter_id == expiryBatchID);

        var row = await query.ExecuteUpdateAsync(s => s.SetProperty(e => e.Product_update, e => status));
        return (row > 0);
    }

    //public async Task<bool> InsertProductAmout(Product_Amount item)
    //{
    //    //var item = context.Product_Amount.AsNoTracking().ProductsOrderBy(x => x.Counter_id).LastOrDefault(x => x.Product_id == 22);
    //    //Product_Amount? insertedRow;
    //    if (item != null)
    //    {
    //        item.Counter_id = ++item.Counter_id;
    //        item.Pa_id = 0;
    //        /*
    //        var pro = new Product_Amount()
    //        {
    //            Amount = item.Amount,
    //            Counter_id = ++item.Counter_id,
    //            Product_id = item.Product_id,
    //            Batch_num = item.Batch_num,
    //            Buy_price = item.Buy_price,
    //            Exp_date = item.Exp_date,
    //            Insert_date = item.Insert_date,
    //            Insert_uid = item.Insert_uid,
    //            Product_update = item.Product_update,
    //            Product_update_date = item.Product_update_date,
    //            Sell_price = item.Sell_price,
    //            Store_id = item.Store_id,
    //            Tax_price = item.Tax_price,
    //            Update_date = item.Update_date,
    //            Update_uid = item.Update_uid,
    //            Vendor_id = item.Vendor_id
    //        }; */
    //        context.Add(item);
    //        var effectedRow = await context.SaveChangesAsync();
    //        if (effectedRow > 0)
    //            return true;
    //    }
    //    return false;
    //}

    public async Task<Result> CopyProductAmout(int id, int productId, decimal quantity, DateTime? expDate, string empId)
    {
        var expiryBatchID = context.Product_Amount.Count(x => x.Product_id == productId);

        var product = context.Product_Amount
            .AsNoTracking()
            .FirstOrDefault(x => x.Pa_id == id);

        if (product == null)
            return Result.Failure();

        product.Pa_id = 0;
        product.Amount = quantity;
        product.Counter_id = ++expiryBatchID;
        product.Exp_date = expDate;
        product.Insert_uid = empId;
        product.Update_uid = empId;
        product.Product_update = "1";

        context.Product_Amount.Add(product);
        var effectedRow = await context.SaveChangesAsync();
        if (effectedRow > 0)
        {
            return Result.Success($"{effectedRow} row effected");
        }

        return Result.Failure();
    }

    public async Task<Result> UpdateQuantity(int id, decimal oldQuantity, decimal newQuantity, DateTime? expDate, string empId)
    {
        var product = await context.Product_Amount
            .FirstOrDefaultAsync(x => x.Pa_id == id);

        if (product == null)
            return Result.Failure(ErrorCode.NotFoundById);

        product.Amount = (newQuantity - oldQuantity) + product.Amount;
        product.Exp_date = expDate ?? expDate;
        product.Insert_uid = empId;
        product.Update_uid = empId;
        product.Product_update = "1";
        product.Update_date = DateTime.Now;
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
                .AnyAsync(x => x.Exp_date == date && x.Product_id == productId && x.Pa_id != id);

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
    //            .Any(x => x.Exp_date == date && x.Product_id == productId);
    //        if (result)
    //            return new ContextResponse(result, "Exp_date is Existing");
    //        return new ContextResponse(false, "Exp_date is not Existing");
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
            .Where(p => p.Amount > 0 && p.Store_id == storeId)
            .CountAsync();
        return totalProduct;
    }

    public async Task<int> GetCountAllExpiredProducts(int storeId)
    {
        var totalProduct = await context.Product_Amount
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
            .Where(p => p.Amount > 0 && p.Product_update == "1" && p.Store_id == storeId)
            .CountAsync();
        return totalProduct;
    }
    #endregion

    // The code that's violating the rule is on this line.
    #region Methods for creating queries
#pragma warning disable CA1822
    private string BringSelectAllProductsIncludeJoinQuery(bool isGroup)
    {
        if (isGroup)
        {
            return @$" SELECT {columnsStatement}
                    FROM (SELECT Store_id, Product_id, SUM(Amount) AS Amount, Sell_price 
                        FROM Product_Amount GROUP BY Product_id,Sell_price,Store_id) 
                        AS pa 
                        INNER JOIN Products p ON pa.Product_id = p.Product_id ";
        }
        else
        {
            return @$" SELECT {columnsStatement}
                        FROM Products p
                        INNER JOIN Product_Amount pa ON p.Product_id = pa.Product_id ";
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

    private string BringPaginationQuery(int page, int size = 30)
    {
        var num = (page < 2) ? 0 : (page - 1) * size;
        return $"OFFSET {num} ROWS FETCH NEXT {size} ROWS ONLY ";
    }

    private string BringSearchQuery(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            if (Validator.IsNumeric(searchText))
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

    private string? ExtractBarcode(string input)
    {
        string[] words = input.Split('-', '$');
        return words[0];
    }

    //string GetBarcodeQuery()
    //{

    //    return "";
    //}
#pragma warning restore CA1822
    #endregion

}