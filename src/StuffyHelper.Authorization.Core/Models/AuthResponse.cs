namespace StuffyHelper.Authorization.Core.Models
{
    public class AuthResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int Total { get; set; }
    }
}
