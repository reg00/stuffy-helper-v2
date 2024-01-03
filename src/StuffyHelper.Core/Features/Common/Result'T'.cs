namespace StuffyHelper.Core.Features.Common
{
    public class Result<T> : Result
    {
        public T? Value { get; set; }

        protected internal Result(T? value, bool success, string error) : base(success, error)
        {
            Value = value;
        }
    }
}
