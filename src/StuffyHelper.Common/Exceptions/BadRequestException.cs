using Microsoft.AspNetCore.Mvc.ModelBinding;
using StuffyHelper.Common.Helpers;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Bad request exception
/// 400 status code
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public BadRequestException(ModelStateDictionary model)
    : base(model.ConvertToError())
    {
    }
}