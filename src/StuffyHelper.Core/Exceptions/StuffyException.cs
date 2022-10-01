namespace StuffyHelper.Core.Exceptions
{
    public class StuffyException : Exception
    {
        public StuffyException(string message) : base(message) { }

        public StuffyException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
