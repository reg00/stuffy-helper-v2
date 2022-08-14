namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class EntityAlreadyExistsException : AuthorizationException
    {
        public EntityAlreadyExistsException(string message)
                       : base(message)
        {
        }

        public EntityAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
