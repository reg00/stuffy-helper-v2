namespace StuffyHelper.Minio.Features.Exceptions
{
    public class FileTypeNotSupportedException : Exception
    {
        public FileTypeNotSupportedException(string message) : base(message)
        {
        }
    }
}
