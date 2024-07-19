using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using System.Text.RegularExpressions;

namespace DataAccess.Repository;
public class ProductAmountRepo
{
    private readonly AppDb context;
    const string whereStatement = " WHERE p.deleted = 1 AND p.active = 1 ";
    const string columnsStatement = @" p.product_id AS [ProductId], p.product_int_code AS [InternationalCode], ISNULL(NULLIF(p.product_name_en, ''), p.product_name_ar) AS [Name], p.sell_price AS [SalePrice], pa.amount AS [Quantity], pa.store_id AS [StoreId] ";

    public ProductAmountRepo()
    {
        context = new AppDb();
    }

    public List<ProductDto> GetAllProducts(ProductQueryParameters qParam)
    {
        string selectClause = SelectionClause(qParam.IsGroup, qParam.AllowDuplicates);
        string whereQuantity = qParam.QuantityBiggerThanZero ? " AND pa.amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(qParam.StoreId) ? "" : $" AND pa.store_id = {qParam.StoreId} ";
        string whereSiteId = string.IsNullOrEmpty(qParam.SiteId) ? "" : $" AND products.site_id = {qParam.SiteId} ";
        string whereOrderBy = $" ORDER BY {GetOrderByQuery((OrderBy)qParam.OrderBy)}";
        string whereSearchText = GetSearchQuery(qParam.Text);
        string offset = GetPaginationQuery(qParam.Page);
        string sql = @$"{selectClause} 
                        {whereStatement}
                        {whereQuantity}
                        {whereSearchText}
                        {whereStoreId}
                        {whereSiteId}
                        {whereOrderBy}
                        {offset}";
        var list = context.Database
            .SqlQueryRaw<ProductDto>(sql)
            .ToList();
        return list;
    }

    public List<ProductDetailsDto> GetProductDetailsLinq(bool hasQuantity, decimal? productId, decimal? storeId)
    {
        var query = from p in context.Products
                    join pAmount in context.Product_Amount on p.product_id equals pAmount.product_id
                    join company in context.Companys on p.company_id equals company.company_id
                    join productUnit in context.Product_units on p.product_unit1 equals productUnit.unit_id
                    join vendor in context.Vendor on pAmount.vendor_id equals vendor.vendor_id into vendorGroup
                    from vendor in vendorGroup.DefaultIfEmpty()
                    where p.deleted == "1" && p.active == "1"
                    orderby pAmount.exp_date descending
                    select new ProductDetailsDto
                    {
                        ProductId = pAmount.product_id,
                        StoreId = pAmount.store_id,
                        CounterId = pAmount.counter_id,
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
        if (hasQuantity)
            query = query.Where(q => q.Amount > 0);
        if (storeId.HasValue)
            query = query.Where(s => s.StoreId == 1);
        query = query.Where(p => p.ProductId == productId);
        var result = query.ToList();
        return result;
    }

    public List<ProductDetailsDto> GetProductDetails(bool hasQuantity, string productId, string storeId)
    {
        string whereQuantity = hasQuantity ? " AND pAmount.amount > 0 " : "";
        string whereStoreId = string.IsNullOrEmpty(storeId) ? "" : $" AND pAmount.store_id = {storeId} ";
        string sql = @$"SELECT
                        pAmount.product_id AS [ProductId],
                        pAmount.store_id AS [StoreId],
                        pAmount.counter_id AS [CounterId],
                        pAmount.vendor_id AS [VendorId],
                        pAmount.exp_date AS [ExpDate],
                        pAmount.amount AS [Amount],
                        pAmount.sell_price AS [SellPrice],
                        pAmount.product_update AS [IsInventoried],
                        p.product_name_ar AS [ProductNameAr],
                        p.product_name_en AS [ProductNameEn],
                        p.product_unit1 AS [ProductUnit1],
                        p.product_unit1_3 AS [ProductUnit13],
                        vendor.vendor_name_ar AS [VendorNameAr],
                        companys.co_name_ar AS [CompanyNameAr]
                    FROM products as p
                        INNER JOIN Product_Amount as pAmount ON p.product_id = pAmount.product_id  
                        INNER JOIN companys ON p.company_id = companys.company_id 
                        INNER JOIN product_units ON p.product_unit1 = product_units.unit_id 
                        LEFT OUTER JOIN vendor ON pAmount.vendor_id = vendor.vendor_id 
                    {whereStatement} 
                    {whereStoreId} 
                     AND pAmount.product_id = {productId} 
                     {whereQuantity} 
                     order by pAmount.counter_id desc";

        var list = context.Database
            .SqlQueryRaw<ProductDetailsDto>(sql)
            .Where(x => x.ProductId == 22)
            .ToList();
        return list;
    }


    string SelectionClause(bool isGroup, bool allowDuplicates)
    {
        var isDistinct = allowDuplicates ? " " : " DISTINCT ";
        if (isGroup)
        {
            return @$" SELECT {columnsStatement}
                        FROM (SELECT store_id, product_id, SUM(amount) AS amount, ISNULL(SUM(amount * buy_price) / NULLIF(SUM(amount), 0), AVG(buy_price)) AS buy_price 
                                FROM Product_Amount GROUP BY product_id, store_id) 
                                AS pa
                        INNER JOIN Products p ON pa.product_id = p.product_id ";
        }
        else
        {
            return @$" SELECT {isDistinct} {columnsStatement}
                        FROM Products p
                        INNER JOIN 
                            Product_Amount pa ON p.product_id = pa.product_id ";
        }

    }
    string GetOrderByQuery(OrderBy orderBy)
    {
        string clause;
        switch (orderBy)
        {
            case OrderBy.Non:
                clause = "(SELECT 1)";
                break;
            case OrderBy.ProductId:
                clause = "p.product_id";
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

    string GetPaginationQuery(int page, int size = 30)
    {
        var num = (page < 2) ? 0 : (page - 1) * size;
        return $"OFFSET {num} ROWS FETCH NEXT {size} ROWS ONLY ";
    }

    string GetSearchQuery(string searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            //var containsEnglishLetters = Regex.IsMatch(searchText, @"[a-zA-Z]");
            var isNumeric = Regex.IsMatch(searchText, @"^\d+$");

            if (isNumeric)
            {
                return $"AND p.product_id  ={searchText} or p.product_int_code = '{searchText}'";
            }
            else
            {
                return $" AND (p.product_name_en  LIKE '%{searchText}%' or p.product_name_ar  LIKE '%{searchText}%') ";
            }
        }
        return " ";
    }

    string GetBarcodeQuery()
    {

        return "";
    }

    public bool UpdateInventoryStatus(bool allOneTime, string status, int? productId, int? batchId)
    {
        //var row = context.Product_Amount
        //    .Where(x => x.product_id == productId)
        //    //.Where(u => u.counter_id == batchId )
        //    .ExecuteUpdate(s => s.SetProperty(e => e.Product_update, e => status));

        var query = context.Product_Amount
            .Where(x => x.product_id == productId);
        if (!allOneTime)
            query = query.Where(u => u.counter_id == batchId);
        var row = query.ExecuteUpdate(s => s.SetProperty(e => e.Product_update, e => status));
        return (row > 0);
    }
}

public enum OrderBy
{
    Non = 0,
    ProductId = 1,
    Name = 2,
    BiggestPrice = 3,
    LowestPrice = 4,

}
