namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Authorization exception
/// 401 status code
/// </summary>
public class AuthorizationException : Exception
{
    public AuthorizationException(string message)
        : base(message)
    {
    }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}