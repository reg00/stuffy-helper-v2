namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class AuthorizationResourceNotFoundException : Exception
    {
        public AuthorizationResourceNotFoundException(string message)
                       : base(message)
        {
        }

        public AuthorizationResourceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
