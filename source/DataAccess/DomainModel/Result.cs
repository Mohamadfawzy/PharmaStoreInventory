// this file has 2 classes Result<T> and Result
namespace DataAccess.DomainModel;
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorCode ErrorCode { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static Result<T> Success(T data, string message = "Success")
    {
        return new Result<T> { IsSuccess = true, Data = data, Message = message };
    }


    //public static Result<T> Failure(IEnumerable<string> errors, string message = "")
    //{
    //    return new Result<T> { IsSuccess = false, Errors = errors, Message = message };
    //}

    public static Result<T> Failure(string message = "Failure")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message
        };
    }

    //public static Result<T>? Failure(string message, params string[] errors )
    //{
    //    if (errors == null)
    //        return default;

    //    var list = new List<string>();
    //    foreach (var error in errors)
    //    {
    //        list.Add(error);
    //    }

    //    return new Result<T>
    //    {
    //        IsSuccess = false,
    //        Errors = errors,
    //        Message = message
    //    };
    //}
    
    public static Result<T> Failure(ErrorCode errorCode,string message= "Failure")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
}





public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorCode ErrorCode { get; set; }
    public IEnumerable<string>? Errors { get; set; } 

    public static Result Success(string message = "IsSuccess")
    {
        return new Result { IsSuccess = true, Message = message };
    }

    public static Result Failure(IEnumerable<string> errors, string message = "")
    {
        return new Result { IsSuccess = false, Errors = errors, Message = message };
    }

    public static Result Failure(string message, params string[] errors)
    {
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

    public static Result Failure(string message ="Failure")
    {
        return new Result
        {
            IsSuccess = false,
            Message = message
        };
    }
    
    public static Result Exception(string message)
    {
        return new Result
        {
            IsSuccess = false,
            Message = $"An error occurred: {message}"
        };
    }
    
    public static Result Failure(ErrorCode errorCode, string message ="Failure")
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
}
