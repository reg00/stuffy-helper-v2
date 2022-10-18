namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class AuthorizationEntityAlreadyExistsException : AuthorizationException
    {
        public AuthorizationEntityAlreadyExistsException(string message)
                       : base(message)
        {
        }

        public AuthorizationEntityAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
