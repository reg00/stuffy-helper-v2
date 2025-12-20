using System.Net;
using StuffyHelper.Common.Constants;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Database exception
/// 500 status code
/// </summary>
public class DbStoreException : BaseException
{
    public DbStoreException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.InternalServerError, ErrorCodeConstants.DbStoreErrorCode, args)
    {
    }

    public DbStoreException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.InternalServerError, ErrorCodeConstants.DbStoreErrorCode, innerException, args)
    {
    }

    public DbStoreException(Exception innerException)
        : base("The data store operation failed.", HttpStatusCode.InternalServerError, ErrorCodeConstants.DbStoreErrorCode, innerException)
    {
    }
}