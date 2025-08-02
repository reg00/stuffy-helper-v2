namespace StuffyHelper.Minio.Features.Exceptions
{
    /// <summary>
    /// Stuffy minio extensions
    /// </summary>
    public class StuffyMinioException : Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public StuffyMinioException(string message) : base(message)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public StuffyMinioException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public StuffyMinioException(Exception innerException)
            : base("The minIO data store operation failed.", innerException)
        {
        }
    }
}
