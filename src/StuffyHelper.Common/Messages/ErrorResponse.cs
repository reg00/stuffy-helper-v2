using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StuffyHelper.Common.Messages;

/// <summary>
/// Error response
/// </summary>
public class ErrorResponse
{
    public string Message { get; init; } = string.Empty;

    public Dictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();

    public ErrorResponse()
    {
    }

    public ErrorResponse(ModelStateDictionary model)
    {
        Errors = model.Where(x => x.Value != null && x.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
    }
}