namespace StuffyHelper.Core.Features.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public string Error { get; private set; }
        public bool IsFailure => !Success;

        protected Result(bool success, string error)
        {
            if (success && !string.IsNullOrWhiteSpace(error) ||
               !success && string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException();

            Success = success;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }
}
