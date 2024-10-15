using Shared.Enums;
using System.Net;

namespace Shared.MAUI.Models;

public class ProviderResult<T>
{
    public bool IsSuccess { get; set; } 
    public T? Content { get; set; }
    public HttpStatusCode? HttpStatusCode { get; set; }
    public ErrorException? ErrorException { get; set; }

    public static ProviderResult<T> Success(T? data)
    {
        return new ProviderResult<T>
        {
            IsSuccess = true,
            Content = data,
        };
    }

    public static ProviderResult<T> Failure(HttpStatusCode httpStatusCode)
    {
        return new ProviderResult<T>
        {
            IsSuccess = false,
            HttpStatusCode = httpStatusCode,
            ErrorException = null,
        };
    }
    
    public static ProviderResult<T> Failure(ErrorException errorException)
    {
        return new ProviderResult<T>
        {
            IsSuccess = false,
            ErrorException = errorException,
            HttpStatusCode = null
        };
    }
}