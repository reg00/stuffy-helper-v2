using System.Text;
using System.Text.Json;
using RestSharp;
using RestSharp.Serializers.Json;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Helpers;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Common.Client;

/// <summary>
/// Base HTTP client based on RestSharp client
/// </summary>
public abstract class ApiClientBase
{
    private static readonly Lazy<JsonSerializerOptions> SerializerOptions;

    static ApiClientBase()
    {
        SerializerOptions = new Lazy<JsonSerializerOptions>(() => JsonOptionsFactory.DefaultOptions);
    }

    private readonly RestClient _client;

    protected ApiClientBase(string baseUrl)
    {
        _client = new RestClient(new RestClientOptions(baseUrl)
            {
                ThrowOnDeserializationError = true,
                Timeout = TimeSpan.MaxValue
            },
            configureSerialization: s => s.UseSystemTextJson(SerializerOptions.Value));
    }

    /// <summary>
    /// Performs a GET request to the target endpoint.
    /// </summary>
    protected Task<T> Get<T>(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute<T>(request, Method.Get, cancellationToken);
    }

    /// <summary>
    /// Performs a POST request to the target endpoint.
    /// </summary>
    protected Task<T> Post<T>(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute<T>(request, Method.Post, cancellationToken);
    }

    /// <summary>
    /// Performs a PUT request to the target endpoint.
    /// </summary>
    protected Task<T> Put<T>(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute<T>(request, Method.Put, cancellationToken);
    }
    
    /// <summary>
    /// Performs a PATCH request to the target endpoint.
    /// </summary>
    protected Task<T> Patch<T>(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute<T>(request, Method.Patch, cancellationToken);
    }

    /// <summary>
    /// Performs a DELETE request to the target endpoint.
    /// </summary>
    protected Task<T> Delete<T>(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute<T>(request, Method.Delete, cancellationToken);
    }

    /// <summary>
    /// Performs a specified request to the target endpoint and handles the errors.
    /// </summary>
    protected async Task<T> Execute<T>(RestRequest request, Method method, CancellationToken cancellationToken)
    {
        var response = await ExecuteInternal(request, method, cancellationToken);

        T? result;
        try
        {
            result = (await _client.Deserialize<T>(response, cancellationToken)).Data;
        }
        catch (Exception e)
        {
            throw CreateSerializationException(e, request, response);
        }

        return result ?? throw CreateSerializationException(null, request, response);
    }

    /// <summary>
    /// Performs a GET request to the target endpoint.
    /// </summary>
    protected Task Get(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute(request, Method.Get, cancellationToken);
    }

    /// <summary>
    /// Performs a POST request to the target endpoint.
    /// </summary>
    protected Task Post(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute(request, Method.Post, cancellationToken);
    }

    /// <summary>
    /// Performs a PUT request to the target endpoint.
    /// </summary>
    protected Task Put(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute(request, Method.Put, cancellationToken);
    }

    /// <summary>
    /// Performs a DELETE request to the target endpoint.
    /// </summary>
    protected Task Delete(RestRequest request, CancellationToken cancellationToken)
    {
        return Execute(request, Method.Delete, cancellationToken);
    }

    /// <summary>
    /// Performs a GET request that handles file receiving.
    /// </summary>
    protected async Task<FileParam> GetFile(RestRequest request, CancellationToken cancellationToken)
    {
        var response = await ExecuteInternal(request, Method.Get, cancellationToken);

        var bytes = response.RawBytes ?? throw new HttpException("Failed to extract file from response");
        
        var fileName = "file";
        if (response.TryExtractFileName(out var name))
        {
            fileName = name ?? fileName;
        }

        return new FileParam(bytes, fileName);
    }

    /// <summary>
    /// Performs a POST request that handles file receiving.
    /// </summary>
    protected async Task<FileParam> PostFile(RestRequest request, CancellationToken cancellationToken)
    {
        var response = await ExecuteInternal(request, Method.Post, cancellationToken);

        var bytes = response.RawBytes ?? throw new HttpException("Failed to extract file from response");
        
        var fileName = "file";
        if (response.TryExtractFileName(out var name))
        {
            fileName = name ?? fileName;
        }

        return new FileParam(bytes, fileName);
    }

    /// <summary>
    /// Performs a specified request to the target endpoint and handles the errors.
    /// </summary>
    protected async Task Execute(RestRequest request, Method method, CancellationToken cancellationToken)
    {
        await ExecuteInternal(request, method, cancellationToken);
    }

    private async Task<RestResponse> ExecuteInternal(
        RestRequest request,
        Method method,
        CancellationToken cancellationToken)
    {
        var response = await _client.ExecuteAsync(request, method, cancellationToken);

        if (!response.IsSuccessful)
        {
            if (response.Content == null)
            {
                throw CreateHttpException(request, response);
            }

            throw CreateHttpException(request, response);
        }

        return response;
    }

    /// <summary>
    /// Creates a new request. May be extended with additional headers. 
    /// </summary>
    protected virtual RestRequest CreateRequest(string resource)
    {
        return new RestRequest(resource);
    }

    private static SerializationException CreateSerializationException(
        Exception? exception,
        RestRequest request,
        RestResponse response)
    {
        return new SerializationException(
            string.Format("Failed to deserialize response from {Resource}: {Content}", request.Resource,
            response.Content ?? "<empty>"), exception);
    }

    private HttpException CreateHttpException(RestRequest request, RestResponse response)
    {
        var args = new List<object> {request.Resource, (int) response.StatusCode, response.StatusCode};
        var builder = new StringBuilder("Error while calling {Resource}: {ErrorCode} {ErrorName}");

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            args.Add(response.ErrorMessage);
            builder.Append(" {ErrorMessage}");
        }

        builder.Append(". There was no valid ApiError object in response.");

        if (!string.IsNullOrEmpty(response.Content))
        {
            args.Add(response.Content);
            builder.Append(" Raw response: {Raw}");
        }

        return new HttpException(string.Format(builder.ToString(), args.ToArray()));
    }
}

public static class ApiClientExtensions
{
    public static RestRequest AddInstantQueryParameter(this RestRequest request, string name, DateTime? instant)
    {
        return instant.HasValue ? request.AddQueryParameter(name, instant.Value.ToString()) : request;
    }

    /// <summary>
    /// Adds optional pagination information to the request.
    /// </summary>
    public static RestRequest AddPagination(this RestRequest request, PaginationRequest? pagination)
    {
        if (pagination != null)
        {
            request.AddQueryParameter("page", pagination.Page);
            request.AddQueryParameter("pagesize", pagination.PageSize);
        }

        return request;
    }

    /// <summary>
    /// Adds optional query parameter to the request.
    /// </summary>
    public static RestRequest AddOptionalQueryParameter<T>(
        this RestRequest request,
        string name,
        T? value,
        bool encode = true) where T : struct
    {
        return value.HasValue ? request.AddQueryParameter(name, value.Value, encode) : request;
    }

    /// <summary>
    /// Adds optional query parameter to the request.
    /// </summary>
    public static RestRequest AddOptionalQueryParameter(
        this RestRequest request,
        string name,
        string? value,
        bool encode = true)
    {
        return value != null ? request.AddQueryParameter(name, value, encode) : request;
    }

    /// <summary>
    /// Adds optional query parameter to the request.
    /// </summary>
    public static RestRequest AddOptionalQueryParameter(
        this RestRequest request,
        string name,
        string[]? values,
        bool encode = true)
    {
        if (values == null)
            return request;

        foreach (var value in values)
        {
            request.AddQueryParameter(name, value, encode);
        }

        return request;
    }

    /// <summary>
    /// Adds optional query parameter to the request.
    /// </summary>
    public static RestRequest AddOptionalQueryParameter<T>(
        this RestRequest request,
        string name,
        T[]? values,
        bool encode = true) where T : struct
    {
        if (values == null)
            return request;

        foreach (var value in values)
        {
            request.AddQueryParameter(name, value.ToString(), encode);
        }

        return request;
    }

    /// <summary>
    /// Adds file parameter to the request (multipart/form-data).
    /// </summary>
    public static RestRequest AddFile(
        this RestRequest request,
        string name,
        FileParam parameter)
    {
        return request.AddFile(name, parameter.Content, parameter.FileName);
    }

    /// <summary>
    /// Adds Authorization header with Bearer token.
    /// </summary>
    public static RestRequest AddBearerToken(this RestRequest request, string token)
    {
        return request.AddOrUpdateHeader("Authorization", $"Bearer {token}");
    }
}