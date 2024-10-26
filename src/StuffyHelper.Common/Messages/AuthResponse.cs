namespace StuffyHelper.Common.Messages;

public class AuthResponse<T>
{
    public IEnumerable<T> Data { get; init; } = Enumerable.Empty<T>();
    public int TotalPages { get; init; }
    public int Total { get; init; }
}