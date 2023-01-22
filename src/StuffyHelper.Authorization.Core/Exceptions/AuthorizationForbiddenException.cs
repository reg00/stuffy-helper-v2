namespace StuffyHelper.Authorization.Core.Exceptions
{
    public class AuthorizationForbiddenException : Exception
    {
        public AuthorizationForbiddenException(string message)
                       : base(message)
        {
        }

        public AuthorizationForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
