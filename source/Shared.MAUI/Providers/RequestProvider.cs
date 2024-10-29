using Shared.MAUI.Providers;
using Shared.Utilities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.MAUI.Services;

public static class RequestProvider
{
    private const short timeoutInSeconds = 20;

    public static async Task<ProviderResult<List<TResult>>> GetAllAsync<TResult>(string uri)
    {
        ProviderResult<List<TResult>> providerResult = new();
        try
        {
            var client = new HttpClient();

            client.Timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                providerResult.HttpStatusCode = response.StatusCode;
            }

            var content = await response.Content.ReadFromJsonAsync<List<TResult>?>().ConfigureAwait(false);
            return ProviderResult<List<TResult>>.Success(content);
        }
        //Occurs when request timeout
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.TaskCanceledException;
        }
        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.HttpRequestException;
        }
        // Occurs when the content not being supported (e.g., JSON content could not be read)
        catch (NotSupportedException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.NotSupportedException;
        }
        // Occurs when the JSON deserialization errors
        catch (JsonException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.JsonException;
        }
        // Occurs when the any other unexpected exceptions
        catch (Exception ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.Exception;
        }
        return providerResult;
    }

    public static async Task<ProviderResult<TResult?>> PostSingleAsync<TResult, TTake>(string uri, TTake data, string token = "")
    {
        ProviderResult<TResult?> providerResult = new();

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(timeoutInSeconds) };
        using var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = JsonContent.Create(data) 
        };

        try
        {
            // Send the POST request and ensure the response status code indicates success
            using var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<TResult?>().ConfigureAwait(false);
            return ProviderResult<TResult?>.Success(content);
        }

        //Occurs when request timeout
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.TaskCanceledException;
        }
        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.HttpRequestException;
        }
        // Occurs when the content not being supported (e.g., JSON content could not be read)
        catch (NotSupportedException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.NotSupportedException;
        }
        // Occurs when the JSON deserialization errors
        catch (JsonException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.JsonException;
        }
        // Occurs when the any other unexpected exceptions
        catch (Exception ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.Exception;
        }
        return providerResult;
    }


    public static async Task<ProviderResult<TResult?>> PutByQueryParamsAsync<TResult>(string uri)
    {
        ProviderResult<TResult?> providerResult = new();

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<TResult?>().ConfigureAwait(false);
            return ProviderResult<TResult?>.Success(content);
        }
        //Occurs when request timeout
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.TaskCanceledException;
        }
        // Occurs when the HTTP response code is not successful (like 404, 500, etc.)
        catch (HttpRequestException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.HttpRequestException;
        }
        // Occurs when the content not being supported (e.g., JSON content could not be read)
        catch (NotSupportedException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.NotSupportedException;
        }
        // Occurs when the JSON deserialization errors
        catch (JsonException ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.JsonException;
        }
        // Occurs when the any other unexpected exceptions
        catch (Exception ex)
        {
            ExceptionLogger.LogException(ex);
            providerResult.ErrorException = Enums.ErrorException.Exception;
        }
        return providerResult;

    }

}