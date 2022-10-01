namespace StuffyHelper.Core.Features.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
