namespace PharmaStoreInventory.Helpers;

public class Configuration
{
    public static async Task<string> ConfigureBaseUrl(string ipAddress, string port) =>
        await Task.FromResult($"http://{ipAddress}:{port}/api");
}
