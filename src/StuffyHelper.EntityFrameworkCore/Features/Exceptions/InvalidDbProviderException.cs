namespace StuffyHelper.EntityFrameworkCore.Features.Exceptions
{
    public class InvalidDbProviderException : Exception
    {
        public InvalidDbProviderException()
            : base("An error occured while trying to parse db provider from configuration.")
        {
        }
    }
}
