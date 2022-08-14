namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class AuthStoreException : Exception
    {
        public AuthStoreException(string message) : base(message)
        {
        }

        public AuthStoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AuthStoreException(Exception innerException)
            : base("The data store operation failed.", innerException)
        {
        }
    }
}
