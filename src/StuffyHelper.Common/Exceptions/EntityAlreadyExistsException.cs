using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Entity already exists exception
/// 409 status code
/// </summary>
public class EntityAlreadyExistsException : BaseException
{
    public EntityAlreadyExistsException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.Conflict, ErrorCodeConstants.EntityAlreadyExistsErrorCode, args)
    {
    }

    public EntityAlreadyExistsException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.Conflict, ErrorCodeConstants.EntityAlreadyExistsErrorCode, innerException, args)
    {
    }
}