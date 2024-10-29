namespace Shared.MAUI.Builders;

public static class QueryBuilder
{
    public static string BuildQueryString(Dictionary<string, string?> queryParams)
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
        catch
        {
            return string.Empty;
        }
    }
}
