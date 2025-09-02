namespace StuffyHelper.Common.Messages;

/// <summary>
/// Error response for API
/// </summary>
public class ApiError
{
    /// <summary>
    /// Error code
    /// </summary>
    public string ErrorCode { get; init; }
    
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; init; }
    
    /// <summary>
    /// Error message template (for logs)
    /// </summary>
    public string MessageTemplate { get; init; }
    
    /// <summary>
    /// Error message template agruments
    /// </summary>
    public object[] Args { get; init; }
    
    /// <summary>
    /// HTTP status code
    /// </summary>
    public int HttpStatus { get; init; }
}