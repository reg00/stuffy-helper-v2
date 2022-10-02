using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StuffyHelper.Core.Features.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public Dictionary<string, string[]> Errors { get; set; }

        public ErrorResponse()
        {
        }

        public ErrorResponse(ModelStateDictionary model)
        {
            Errors = model.Where(x => x.Value.Errors.Count > 0)
                          .ToDictionary(
                              kvp => kvp.Key,
                              kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                          );
        }
    }
}
