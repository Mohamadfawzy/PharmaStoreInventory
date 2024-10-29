using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Languages;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PharmaStoreInventory.Services;

public static class RequestProvider
{
    private const short timeoutInSeconds = 20;
    public static async Task<List<TResult>?> GetAllAsync3<TResult>(string uri)
    {
        ErrorMessage message;

        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);

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

    public static async Task<(List<TResult>? content, ErrorMessage? error)> GetAllAsync<TResult>(string uri)
    {
        ErrorMessage? message = null;

        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return (default, message);
            }

            List<TResult>? result = await response.Content.ReadFromJsonAsync<List<TResult>?>();
            return (result, message);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            // Handle request timeout
            message = new ErrorMessage()
            {
                Title = AppResources.ApiError_TaskCanceledException,
                Body = ex.Message,
                Error = ExceptionErrorCode.TaskCanceledException,
            };
        }

        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            // Handle specific HTTP request exceptions
            //message = new ErrorMessage(AppResources.ApiError_HttpRequestException, ex.Message);
            message = new ErrorMessage()
            {
                Title = AppResources.ApiError_HttpRequestException,
                Body = ex.Message,
                Error = ExceptionErrorCode.HttpRequestException,
            };
        }
        catch (NotSupportedException ex)
        {
            // Handle content not being supported (e.g., JSON content could not be read)
            //message = new ErrorMessage(AppResources.ApiError_NotSupportedException, ex.Message);
            message = new ErrorMessage()
            {
                Title = AppResources.ApiError_NotSupportedException,
                Body = ex.Message,
                Error = ExceptionErrorCode.NotSupportedException,
            };
        }
        catch (JsonException ex)
        {
            // Handle JSON deserialization errors
            //message = new ErrorMessage(AppResources.ApiError_JsonException, ex.Message);
            message = new ErrorMessage()
            {
                Title = AppResources.ApiError_JsonException,
                Body = ex.Message,
                Error = ExceptionErrorCode.JsonException,
            };
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            //message = new ErrorMessage(AppResources.ApiError_Exception, ex.Message);
            message = new ErrorMessage()
            {
                Title = AppResources.ApiError_Exception,
                Body = ex.Message,
                Error = ExceptionErrorCode.Exception,
            };
        }
        message.Body = $"{nameof(GetAllAsync)} {message.Body}";
        return (default, message);
    }

    public static async Task<IEnumerable<TResult>?> GetAllIEnumerableAsync<TResult>(string uri)
    {
        ErrorMessage message;
        try
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);

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
            request.Headers.Add("Authorization", AppPreferences.Token);

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
        // WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }

    public static async Task<bool> GetBooleanValueAsync(string uri)
    {
        try
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutInSeconds)
            };
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }
        catch
        {
            return false;
        }
    }


    public static async Task<(TResult? content, ErrorMessage? error)> PostSingleAsync<TResult, TTake>(string uri, TTake data, string tokn = "")
    {

        ErrorMessage? message = null;
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutInSeconds) };
            using var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(data) // Serialize the data object to JSON and set it as content
            };
            request.Headers.Add("Authorization", AppPreferences.Token);

            // Send the POST request and ensure the response status code indicates success
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            // Deserialize the response content to the specified TResult type
            var result = await response.Content.ReadFromJsonAsync<TResult?>().ConfigureAwait(false);

            return (result, message);
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
        //WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return (default, message);
    }

    public static async Task<TResult?> PutByQueryParamsAsync<TResult>(string uri)
    {
        ErrorMessage message;

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);
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


        try
        {

            // Use HttpClient as a static/shared instance for better performance and to avoid socket exhaustion
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = JsonContent.Create(data)  // Serialize the data object to JSON and set as content
            };
            request.Headers.Add("Authorization", AppPreferences.Token);
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

    public static async Task<TResult?> DeleteAsync<TResult>(string uri)
    {
        ErrorMessage message;
        try
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            request.Headers.Add("Authorization", AppPreferences.Token);

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
        message.Body = $"{nameof(DeleteAsync)} {message.Body}";
        //WeakReferenceMessenger.Default.Send(new NotificationMessage(message));
        return default;
    }
}
