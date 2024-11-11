namespace StuffyHelper.Common.Messages;

/// <summary>
/// Generic Auth response
/// </summary>
public class Response<T>
{
    public IEnumerable<T> Data { get; init; } = Enumerable.Empty<T>();
    public int TotalPages { get; init; }
    public int Total { get; init; }
}