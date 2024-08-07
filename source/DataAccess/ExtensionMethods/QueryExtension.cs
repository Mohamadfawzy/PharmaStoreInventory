namespace DataAccess.ExtensionMethods;

public static class QueryExtension
{
    public static IQueryable<T> Pagination<T>(this  IQueryable<T>  query, int? page, int? pageSize)
    {
        if (page.HasValue && pageSize.HasValue)
        {
            int skip = (page.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }
        return query;
    }
}
