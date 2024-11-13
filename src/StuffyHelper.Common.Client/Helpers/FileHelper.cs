using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Common.Client.Helpers;

/// <summary>
/// Helper for work with files
/// </summary>
public static class FileHelper
{
    public static FileParam ToFileParam(this IFormFile file)
    {
        using var stream = new MemoryStream();
        file.CopyTo(stream);
        var bytes = stream.ToArray();

        return new FileParam(bytes, file.FileName);
    }
}