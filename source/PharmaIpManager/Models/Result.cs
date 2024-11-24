namespace ApiSettingsManager.Models;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ErrorCode { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
