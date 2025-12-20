using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StuffyHelper.Common.Constants;
using StuffyHelper.Common.Helpers;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Bad request exception
/// 400 status code
/// </summary>
public class BadRequestException : BaseException
{
    public BadRequestException(string messageTemplate, params object[] args)
        : base(messageTemplate, HttpStatusCode.BadRequest, ErrorCodeConstants.BadRequestErrorCode, args)
    {
    }

    public BadRequestException(string messageTemplate, Exception innerException, params object[] args)
        : base(messageTemplate, HttpStatusCode.BadRequest, ErrorCodeConstants.BadRequestErrorCode, innerException, args)
    {
    }

    public BadRequestException(ModelStateDictionary model)
    : base(model.ConvertToError(), HttpStatusCode.BadRequest, ErrorCodeConstants.BadRequestErrorCode)
    {
    }
}