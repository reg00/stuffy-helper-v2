using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Entity not found exception
/// 404 status code
/// </summary>
public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.NotFound, ErrorCodeConstants.EntityNotFoundErrorCode, args)
    {
    }

    public EntityNotFoundException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.NotFound, ErrorCodeConstants.EntityNotFoundErrorCode, innerException, args)
    {
    }
}