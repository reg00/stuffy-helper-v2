using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Authorization exception
/// 401 status code
/// </summary>
public class AuthorizationException : BaseException
{
    public AuthorizationException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.Unauthorized, ErrorCodeConstants.AuthorizationErrorCode, args)
    {
    }

    public AuthorizationException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.Unauthorized, ErrorCodeConstants.AuthorizationErrorCode, innerException, args)
    {
    }
}