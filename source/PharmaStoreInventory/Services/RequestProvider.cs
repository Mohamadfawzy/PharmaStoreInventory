using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Languages;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PharmaStoreInventory.Services;

public static class RequestProvider
{
    // request.Headers.Add("DbConnectionString", Helpers.AppValues.ConnectionString);
    private const short timeoutInSeconds = 10;
    public static async Task<List<TResult>?> GetAllAsync<TResult>(string uri)
    {
        ErrorMessage message;

        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            List<TResult>? result = await response.Content.ReadFromJsonAsync<List<TResult>?>();
            return result;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message);
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(GetAllAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<IEnumerable<TResult>?> GetAllIEnumerableAsync<TResult>(string uri)
    {
        ErrorMessage message;
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            IEnumerable<TResult>? result = await response.Content.ReadFromJsonAsync<IEnumerable<TResult>?>();
            return result;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message);
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(GetAllIEnumerableAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<TResult?> GetSingleAsync<TResult>(string uri)
    {
        ErrorMessage message;
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                // Return default (null) if no content is returned
                return default;
            }
            TResult? result = await response.Content.ReadFromJsonAsync<TResult?>();
            return result;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message); 
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(GetSingleAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<TResult?> PostSingleAsync<TResult, TTake>(string uri, TTake data)
    {
        ErrorMessage message;
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
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            // Deserialize the response content to the specified TResult type

            return await response.Content.ReadFromJsonAsync<TResult?>().ConfigureAwait(false);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message);
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(PostSingleAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<TResult?> PutByQueryParamsAsync<TResult>(string uri)
    {
        ErrorMessage message;

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }
            TResult? result = await response.Content.ReadFromJsonAsync<TResult?>();
            return result;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message);
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(PutByQueryParamsAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<TResult?> PutAsync<TResult, TTake>(string uri, TTake data)
    {
        ErrorMessage message;

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
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            // Deserialize the response content to the specified TResult type
            return await response.Content.ReadFromJsonAsync<TResult>().ConfigureAwait(false);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage(AppResources.ApiError_TaskCanceledException, ex.Message);
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
        }
        message.Body = $"{nameof(PutAsync)} {message.Body}";
        WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }
}
