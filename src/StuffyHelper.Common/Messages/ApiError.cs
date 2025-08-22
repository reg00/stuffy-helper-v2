namespace StuffyHelper.Common.Messages;

public class ApiError
{
    public string ErrorCode { get; init; }
    public string Message { get; init; }
    public string MessageTemplate { get; init; }
    public object[] Args { get; init; }
    public int HttpStatus { get; init; }
}