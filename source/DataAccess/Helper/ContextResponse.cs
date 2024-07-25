namespace DataAccess.Helper;

public class ContextResponse
{
    public ContextResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; set; }
    public string Message { get; set; }
}
