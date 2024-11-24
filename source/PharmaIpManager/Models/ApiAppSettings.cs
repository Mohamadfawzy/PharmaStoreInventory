namespace ApiSettingsManager.Models;

public class ApiAppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string ApplicationUrl { get; set; }
    public string Server { get; set; }
}