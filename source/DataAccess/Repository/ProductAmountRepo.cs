using DataAccess.Contexts;
using DataAccess.DomainModel;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Helper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess.Repository;
public class ProductAmountRepo
{
    private readonly AppDb context;
    const string whereStatement = " WHERE p.deleted = 1 AND p.active = 1 ";
    const string columnsStatement = @" p.product_code AS [ProductCode], p.product_int_code AS [InternationalCode], ISNULL(NULLIF(p.product_name_en, ''), p.product_name_ar) AS [Name], pa.sell_price AS [SalePrice], pa.amount AS [Quantity], pa.store_id AS [StoreId] ";

    public ProductAmountRepo()
    {
        context = new();
    }

    public List<ProductDto> GetAllProducts(ProductQueryParameters qParam)
    {
        string selectClause = BringSelectAllProductsIncludeJoinQuery(qParam.IsGroup);
        string whereQuantity = qParam.QuantityBiggerThanZero ? " AND pa.amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(qParam.StoreId) ? "" : $" AND pa.store_id = {qParam.StoreId} ";
        string whereSiteId = string.IsNullOrEmpty(qParam.SiteId) ? "" : $" AND products.site_id = {qParam.SiteId} ";
        string whereOrderBy = $" ORDER BY {BringOrderByQuery((OrderBy)qParam.OrderBy)}";
        string whereSearchText = BringSearchQuery(qParam.Text);
        string offset = BringPaginationQuery(qParam.Page);
        string sql = @$"{selectClause} 
                        {whereStatement}
                        {whereQuantity}
                        {whereSearchText}
                        {whereStoreId}
                        {whereSiteId}
                        {whereOrderBy}
                        {offset}";
        return context.Database
            .SqlQueryRaw<ProductDto>(sql)
            .ToList();
    }

    public List<ProductDetailsDto>? GetProductDetailsLinq(bool hasQuantity, string barcodeParam, decimal? storeId)
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
                    where (hasQuantity == false || pAmount.amount > 0)
                    where (!storeId.HasValue || pAmount.store_id == storeId)
                    where (p.product_code == barcode
                        //|| p.product_fast_code == barcode
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
                        ExpiryBatchID = pAmount.counter_id,
                        ExpDate = pAmount.exp_date,
                        VendorId = pAmount.vendor_id,
                        Amount = pAmount.amount,
                        SellPrice = pAmount.sell_price,
                        IsInventoried = pAmount.Product_update,
                        ProductNameAr = p.product_name_ar,
                        ProductNameEn = p.product_name_en,
                        ProductUnit1 = p.product_unit1,
                        ProductUnit13 = p.product_unit1_3,
                        VendorNameAr = vendor.vendor_name_ar,
                        CompanyNameAr = company.co_name_ar
                    };
        //if (hasQuantity)
        //    query = query.Where(p => p.Amount > 0);
        //if (storeId.HasValue)
        //    query = query.Where(p => p.StoreId == storeId);

        var result = query.ToList();
        return result;
    }

    public ProductDetailsDto? GetProductById(int id)
    {
        var pro = context.Product_Amount
            .FirstOrDefault(x => x.pa_id == id);
        return (ProductDetailsDto?)pro;
    }

    public bool UpdateInventoryStatus(bool allOneTime, string status, int? productId, int? counterId)
    {
        var query = context.Product_Amount
            .Where(x => x.product_id == productId);
        if (!allOneTime)
            query = query.Where(u => u.counter_id == counterId);
        var row = query.ExecuteUpdate(s => s.SetProperty(e => e.Product_update, e => status));
        return (row > 0);
    }

    public bool InsertProductAmout(Product_Amount item)
    {
        //var item = context.Product_Amount.AsNoTracking().OrderBy(x => x.counter_id).LastOrDefault(x => x.product_id == 22);
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
            var effectedRow = context.SaveChanges();
            if (effectedRow > 0)
                return true;
        }
        return false;
    }

    public ContextResponse CopyProductAmout(int id, int productId, decimal quantity, DateTime? expDate)
    {
        ContextResponse res = new ContextResponse(false, "fail");

        var expiryBatchID = context.Product_Amount.Count(x => x.product_id == productId);

        var product = context.Product_Amount
            .AsNoTracking()
            .FirstOrDefault(x => x.pa_id == id);

        if (product == null)
            return res;

        product.pa_id = 0;
        product.amount = quantity;
        product.counter_id = ++expiryBatchID;
        product.exp_date = expDate;
        product.insert_uid = Helper.Constants.UserId;
        product.update_uid = Helper.Constants.UserId;
        product.Product_update = "1";

        context.Product_Amount.Add(product);
        var effectedRow = context.SaveChanges();
        if (effectedRow > 0)
        {
            res.Success = true;
            res.Message = $"{effectedRow} row effected";
            return res;
        }
        else
            return res;
    }

    public ContextResponse UpdateQuantity(int id, decimal oldQuantity, decimal newQuantity, DateTime? expDate)
    {
        var product = context.Product_Amount
            .FirstOrDefault(x => x.pa_id == id);

        if (product == null)
            return new ContextResponse(false, "fail");

        product.amount = (newQuantity - oldQuantity) + product.amount;
        product.exp_date = expDate ?? expDate;
        product.insert_uid = Helper.Constants.UserId;
        product.update_uid = Helper.Constants.UserId;
        product.Product_update = "1";
        product.update_date = DateTime.Now;
        product.Product_update_date = DateTime.Now;

        if (context.SaveChanges() > 0)
        {
            return new ContextResponse(true, "success");
        };

        return new ContextResponse(false, "not updated");
    }

    public ContextResponse IsExpirationDateExistingExceptOne(int id, int productId, DateTime date)
    {
        try
        {
            var result = context.Product_Amount
                .Any(x => x.exp_date == date && x.product_id == productId && x.pa_id != id);
            if (result)
                return new ContextResponse(result, "exp_date is Existing");
            return new ContextResponse(false, "exp_date is not Existing");
        }
        catch
        {
            return new ContextResponse(false, "catch error"); ;
        }
    }

    public ContextResponse IsExpirationDateExisting(int id, int productId, DateTime date)
    {
        try
        {
            var result = context.Product_Amount
                .Any(x => x.exp_date == date && x.product_id == productId);
            if (result)
                return new ContextResponse(result, "exp_date is Existing");
            return new ContextResponse(false, "exp_date is not Existing");
        }
        catch
        {
            return new ContextResponse(false, "catch error"); ;
        }
    }

    public bool SetAllProductNonInventoried(int storeId)
    {
        var currentDate = DateTime.Now;

        var query = context.Product_Amount
            .Where(b => b.store_id == storeId)
            .ExecuteUpdate(setters => setters
            .SetProperty(b => b.Product_update, "0")
            .SetProperty(b => b.Product_update_date, currentDate)
            .SetProperty(b => b.update_date, currentDate)
            .SetProperty(b => b.update_uid, Helper.Constants.UserId)); 
        return (query > 0);
    }


    #region Statistics
    public int GetCountAllProducts(int storeId)
    {
        var totalProduct = context.Product_Amount
            .Where(p => p.amount > 0 && p.store_id == storeId)
            .Count();
        return totalProduct;
    }

    public int GetCountAllExpiredProducts(int storeId)
    {
        var totalProduct = context.Product_Amount
            .Where(p => p.amount > 0 && p.exp_date < DateTime.Now && p.store_id == storeId)
            .Select(p => p.product_id)
            .Count();
        return totalProduct;
    }

    public int GetCountAllProductsWillExpireAfter3Months(int storeId)
    {
        var currentDate = DateTime.Now;
        var threeMonthsLater = currentDate.AddMonths(3);

        var totalProduct = context.Product_Amount
            .Where(p => p.amount > 0
                        && p.exp_date > currentDate
                        && p.exp_date < threeMonthsLater
                        && p.store_id == storeId)
            .Count();
        return totalProduct;
    }

    public int GetCountAllIsInventoryed(int storeId)
    {
        var totalProduct = context.Product_Amount
            .Where(p => p.amount > 0 && p.Product_update == "1" && p.store_id == storeId)
            .Count();
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

    string BringOrderByQuery(OrderBy orderBy)
    {
        string clause;
        switch (orderBy)
        {
            case OrderBy.Non:
                clause = "(SELECT 1)";
                break;
            case OrderBy.ProductId:
                clause = "p.product_code";
                break;
            case OrderBy.BiggestPrice:
                clause = "p.sell_price desc";
                break;
            case OrderBy.LowestPrice:
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