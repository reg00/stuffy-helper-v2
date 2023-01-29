namespace StuffyHelper.Minio.Features.Exceptions
{
    public class StuffyMinioException : Exception
    {
        public StuffyMinioException(string message) : base(message)
        {
        }

        public StuffyMinioException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public StuffyMinioException(Exception innerException)
            : base("The minIO data store operation failed.", innerException)
        {
        }
    }
}
