using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Serialization exception
/// 400 status code
/// </summary>
public class SerializationException : BaseException
{
    public SerializationException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.BadRequest, ErrorCodeConstants.SerializationErrorCode, args)
    {
    }

    public SerializationException(string messageTemplate, Exception? innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.BadRequest, ErrorCodeConstants.SerializationErrorCode, innerException, args)
    {
    }
}