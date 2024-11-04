namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Entity already exists exception
/// 409 status code
/// </summary>
public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(string message)
        : base(message)
    {
    }

    public EntityAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}