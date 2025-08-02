namespace StuffyHelper.Common.Exceptions
{
    /// <summary>
    /// Stuffy helper exception
    /// 500 status code
    /// </summary>
    public class StuffyException : Exception
    {
        public IDictionary<string, string>? Errors { get; set; } = new Dictionary<string, string>();

        public StuffyException(string message, IDictionary<string, string>? errors = null) : base(message)
        {
            Errors = errors;
        }

        public StuffyException(string message, Exception innerException, IDictionary<string, string>? errors = null)
            : base(message, innerException)
        {
            Errors = errors;
        }
    }
}
