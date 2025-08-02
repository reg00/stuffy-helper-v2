using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StuffyHelper.Common.Helpers;

/// <summary>
/// Helper for works with exceptions
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// Convert to string error
    /// </summary>
    public static string ConvertToError(this ModelStateDictionary model)
    {
        var errors = model.Where(x => x.Value != null && x.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return string.Join('\n', errors);
    }
}