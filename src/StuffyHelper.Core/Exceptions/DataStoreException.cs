namespace StuffyHelper.Core.Exceptions
{
    public class DataStoreException : Exception
    {
        public DataStoreException(string message) : base(message)
        {
        }

        public DataStoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DataStoreException(Exception innerException)
            : base("The data store operation failed.", innerException)
        {
        }
    }
}
