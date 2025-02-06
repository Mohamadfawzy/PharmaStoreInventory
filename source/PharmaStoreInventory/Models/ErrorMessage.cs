namespace PharmaStoreInventory.Models;

public class ErrorMessage() 
{
    public int Code { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public ExceptionErrorCode Error { get; set; }

    public ErrorMessage(string title , string body) : this()
    {
        this.Title = title;
        this.Body = body;

    }

    public ErrorMessage(ExceptionErrorCode error, string title = "Error", string body = "") : this()
    {
        this.Error = error;
        this.Title = title;
        this.Body = body;
    }

}


public enum ExceptionErrorCode
{
    TaskCanceledException,
    HttpRequestException,
    NotSupportedException,
    JsonException,
    Exception
}
