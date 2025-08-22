using System.Net;
using System.Text;
using Serilog.Parsing;
using StuffyHelper.Common.Constants;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Common.Exceptions;

/// <summary>
/// Base exception
/// </summary>
public class BaseException : Exception
{
    public HttpStatusCode HttpStatus { get; }
    public string ErrorCode { get; }
    public string MessageTemplate { get; }
    public object[] Args { get; }
    public override string Message => FormatMessage(MessageTemplate, Args);

    protected BaseException(
        string messageTemplate,
        HttpStatusCode httpStatus,
        string errorCode,
        params object[] args)
        : base(FormatMessage(messageTemplate, args))
    {
        MessageTemplate = messageTemplate;
        HttpStatus = httpStatus;
        ErrorCode = errorCode;
        Args = args;
    }

    protected BaseException(
        string messageTemplate,
        HttpStatusCode httpStatus,
        string errorCode,
        Exception? innerException,
        params object[] args)
        : base(FormatMessage(messageTemplate, args), innerException)
    {
        MessageTemplate = messageTemplate;
        HttpStatus = httpStatus;
        ErrorCode = errorCode;
        Args = args;
    }

    private static string FormatMessage(string messageTemplate, object[] args)
    {
        var parser = new MessageTemplateParser();
        var template = parser.Parse(messageTemplate);
        var format = new StringBuilder();
        var index = 0;

        foreach (var tok in template.Tokens)
        {
            if (tok is TextToken)
                format.Append(tok);
            else
                format.Append("{" + index++ + "}");
        }
        
        try
        {
            return string.Format(format.ToString(), args);
        }
        catch (FormatException)
        {
            // Если шаблон некорректный, возвращаем как есть с значениями
            return $"{messageTemplate} [Values: {string.Join(", ", args)}]";
        }
    }

    public static BaseException FromApiError(ApiError error)
    {
        return error.ErrorCode switch
        {
            ErrorCodeConstants.AuthorizationErrorCode => new AuthorizationException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.BadRequestErrorCode => new BadRequestException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.ClientErrorCode => new ClientException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.DbStoreErrorCode => new DbStoreException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.EntityAlreadyExistsErrorCode => new EntityAlreadyExistsException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.EntityNotFoundErrorCode => new EntityNotFoundException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.ForbiddenErrorCode => new ForbiddenException(error.MessageTemplate, error.Args),
            ErrorCodeConstants.SerializationErrorCode => new SerializationException(error.MessageTemplate, error.Args),
            _ => new BaseException(error.MessageTemplate, (HttpStatusCode)error.HttpStatus, error.ErrorCode, error.Args)
        };
    }
    
    public ApiError ToApiError()
    {
        return new ApiError
        {
            HttpStatus = (int)HttpStatus,
            ErrorCode = ErrorCode,
            MessageTemplate = MessageTemplate,
            Message = Message,
            Args = Args
        };
    }
}