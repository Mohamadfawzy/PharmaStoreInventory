using Shared.Enums;

namespace Shared.Models;

public class Result<T> : Result
{
    public T? Data { get; set; }
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorCode ErrorCode { get; set; }
    public List<ErrorCode>? Errors { get; set; }


    public static Result Success(string message = "IsSuccess")
    {
        return new Result { IsSuccess = true, Message = message };
    }

    public static Result Failure(string message = "Failure")
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

    public static Result Failure(ErrorCode errorCode, string message = "Failure")
    {
        return new Result
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }

    public static Result<T> Success<T>(T? data, string message = "Success")
    {
        return new Result<T> { IsSuccess = true, Data = data, Message = message };
    }

    public static Result<T> Failure<T>(string message = "Failure")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message
        };
    }

    public static Result<T> Failure<T>(ErrorCode errorCode, string message = "Failure")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }

    public static Result<T> Failure<T>(ErrorCode errorCode, T data, string message = "Failure")
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            ErrorCode = errorCode,
            Data = data

        };
    }
}

