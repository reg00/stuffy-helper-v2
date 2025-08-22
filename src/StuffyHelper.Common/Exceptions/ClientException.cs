using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Client exception
/// 500 status code
/// </summary>
public class ClientException : BaseException
{
    public ClientException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.InternalServerError, ErrorCodeConstants.ClientErrorCode, args)
    {
    }

    public ClientException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.InternalServerError, ErrorCodeConstants.ClientErrorCode, innerException, args)
    {
    }
}