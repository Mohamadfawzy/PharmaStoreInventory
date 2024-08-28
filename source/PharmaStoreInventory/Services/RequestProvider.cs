using DataAccess.DomainModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PharmaStoreInventory.Services;

public static class RequestProvider
{
    // request.Headers.Add("DbConnectionString", Helpers.AppValues.ConnectionString);

    public static async Task<List<TResult>?> GetAllAsync<TResult>(string uri)
    {
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);

            List<TResult>? result = await response.Content.ReadFromJsonAsync<List<TResult>?>();
            return result;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(GetAllAsync)}\n Message:{ex.Message}");
            return default;
        }
    }

    public static async Task<TResult?> GetSingleAsync<TResult>(string uri)
    {
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);

            TResult? result = await response.Content.ReadFromJsonAsync<TResult?>();
            return result;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(GetSingleAsync)}, Message:{ex.Message}");
            return default;
        }
    }

    public static async Task<string?> GetStringAsync(string uri)
    {
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(GetStringAsync)}, Message:{ex.Message}");
            return default;
        }
    }

    //public static async Task<TResult?> PostSingleAsync<TResult, TTake>(string uri, TTake data)
    //{
    //    try
    //    {
    //        var client = new HttpClient();
    //        var request = new HttpRequestMessage(HttpMethod.Post, uri);
    //        var content = new StringContent(JsonSerializer.Serialize(data), null, "application/json");
    //        request.Content = content;
    //        var response = await client.SendAsync(request);

    //        TResult? result = await response.Content.ReadFromJsonAsync<TResult?>();
    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(PostSingleAsync)}, Message:{ex.Message}");
    //        return default;
    //    }
    //}

    public static async Task<TResult?> PostSingleAsync<TResult, TTake>(string uri, TTake data)
    {
        // Define the timeout duration for the HTTP request
        const int timeoutInSeconds = 10;
        string message = string.Empty;

        // Use HttpClient with a specified timeout
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutInSeconds) };
        using var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = JsonContent.Create(data) // Serialize the data object to JSON and set it as content
        };

        try
        {
            // Send the POST request and ensure the response status code indicates success
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            // Deserialize the response content to the specified TResult type

            return await response.Content.ReadFromJsonAsync<TResult?>().ConfigureAwait(false);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = $"Request timed out - {ex.Message}";
        }
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = $"HTTP error - {ex.Message}";
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = $"Content type not supported - {ex.Message}";
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = $"JSON error - {ex.Message}";
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = $"An unexpected error occurred - {ex.Message}";
        }

        // Return default (null) if an error occurred
        await Helpers.Alerts.DisplaySnackbar($"RequestProvider, {nameof(PostSingleAsync)}: {message}");
        return default;
    }

    public static async Task<TResult?> PutByQueryParamsAsync<TResult>(string uri)
    {
        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await client.SendAsync(request);
            TResult? result = await response.Content.ReadFromJsonAsync<TResult?>();
            return result;
        }
        catch (Exception ex)
        {
            await Helpers.Alerts.DisplaySnackbar($"{nameof(RequestProvider)}:{nameof(PutByQueryParamsAsync)}, Message:{ex.Message}");
            return default;
        }
    }

    public static async Task<TResult?> PutAsync<TResult, TTake>(string uri, TTake data)
    {
        // Use HttpClient as a static/shared instance for better performance and to avoid socket exhaustion
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = JsonContent.Create(data)  // Serialize the data object to JSON and set as content
        };

        try
        {
            // Send the PUT request and ensure the response status code indicates success
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            // Deserialize the response content to the specified TResult type
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            await Helpers.Alerts.DisplaySnackbar($"{nameof(PutAsync)}: HTTP error - {ex.Message}");
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            await Helpers.Alerts.DisplaySnackbar($"{nameof(PutAsync)}: Content type not supported - {ex.Message}");
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            await Helpers.Alerts.DisplaySnackbar($"{nameof(PutAsync)}: JSON error - {ex.Message}");
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            await Helpers.Alerts.DisplaySnackbar($"{nameof(PutAsync)}: An unexpected error occurred - {ex.Message}");
        }

        // Return default (null) if an error occurred
        return default;
    }
}
