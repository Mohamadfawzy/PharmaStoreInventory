namespace DataAccess.DomainModel;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static Result<T> Success(T data, string message = "")
    {
        return new Result<T> { IsSuccess = true, Data = data, Message = message };
    }

    public static Result<T> Failure(IEnumerable<string> errors, string message = "")
    {
        return new Result<T> { IsSuccess = false, Errors = errors, Message = message };
    }

    public static Result<T> Failure(string message, params string[] errors )
    {
        if (errors == null)
            return default;

        var list = new List<string>();
        foreach (var error in errors)
        {
            list.Add(error);
        }

        return new Result<T>
        {
            IsSuccess = false,
            Errors = errors,
            Message = message
        };
    }
    
    public static Result<T> Failure(string message )
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }

    public static Result Success(string message = "")
    {
        return new Result { IsSuccess = true, Message = message };
    }

    public static Result Failure(IEnumerable<string> errors, string message = "")
    {
        return new Result { IsSuccess = false, Errors = errors, Message = message };
    }

    public static Result Failure(string message, params string[] errors)
    {
        if (errors == null)
            return default;

        var list = new List<string>();
        foreach (var error in errors)
        {
            list.Add(error);
        }

        return new Result
        {
            IsSuccess = false,
            Errors = errors,
            Message = message
        };
    }

    public static Result Failure(string message)
    {
        return new Result
        {
            IsSuccess = false,
            Message = message
        };
    }
}
