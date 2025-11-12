namespace DataAccess.DomainModel;

public class PagedResult<T>
{
    public int ItemsCount { get; set; }
    public List<T>? Data { get; set; } = null;
    public bool? IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public static PagedResult<T> Create(List<T>? list)
    {
        return new PagedResult<T>()
        {
            IsSuccess = true,
            ItemsCount = list?.Count ?? 0,
            Data = list
        };
    }

    public static PagedResult<T> Failure(string message = "Failure")
    {
        return new PagedResult<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
}
