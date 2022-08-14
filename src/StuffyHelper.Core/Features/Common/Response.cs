namespace StuffyHelper.Core.Features.Common
{
    public class Response<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int Total { get; set; }
    }
}
