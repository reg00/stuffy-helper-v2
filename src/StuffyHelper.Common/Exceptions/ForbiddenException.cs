using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Forbidden exception
/// 403 status code
/// </summary>
public class ForbiddenException : BaseException
{
    public ForbiddenException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.Forbidden, ErrorCodeConstants.ForbiddenErrorCode, args)
    {
    }

    public ForbiddenException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.Forbidden, ErrorCodeConstants.ForbiddenErrorCode, innerException, args)
    {
    }
}