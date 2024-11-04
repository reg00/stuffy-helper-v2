namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Http exception
/// 400 status code
/// </summary>
public class HttpException : Exception
{
    public HttpException(string message)
        : base(message)
    {
    }

    public HttpException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}