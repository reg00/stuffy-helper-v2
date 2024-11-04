namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Entity not found exception
/// 404 status code
/// </summary>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}