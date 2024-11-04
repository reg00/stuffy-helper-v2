namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Serialization exception
/// 400 status code
/// </summary>
public class SerializationException : Exception
{
    public SerializationException(string message)
        : base(message)
    {
    }

    public SerializationException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}