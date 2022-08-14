namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class EntityNotFoundException : AuthorizationException
    {
        public EntityNotFoundException(string message)
                       : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
