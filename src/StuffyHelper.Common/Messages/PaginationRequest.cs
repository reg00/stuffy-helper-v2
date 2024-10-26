namespace StuffyHelper.Common.Messages;

public record PaginationRequest(int Page = 1, int PageSize = PaginationRequest.DefaultPageSize)
{
    public const int DefaultPageSize = 10;

    public PaginationRequest NextPage() => new(Page + 1, PageSize);
}

public record Pagination(int TotalCount, int Page = 1, int PageSize = PaginationRequest.DefaultPageSize)
{
    public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
}