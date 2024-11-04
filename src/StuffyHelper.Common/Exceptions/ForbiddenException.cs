namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Forbidden exception
/// 403 status code
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message)
        : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}