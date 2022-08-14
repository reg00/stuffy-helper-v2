namespace StuffyHelper.Core.Exceptions
{
    public class EntityConflictException : Exception
    {
        public EntityConflictException(string message)
                       : base(message)
        {
        }

        public EntityConflictException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
