using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using RestSharp;

namespace StuffyHelper.Common.Helpers;

/// <summary>
/// Helper for work with Http clients
/// </summary>
public static class HttpClientHelper
{
    /// <summary>
    /// Return filename from response
    /// </summary>
    public static bool TryExtractFileName(this RestResponse response, out string? fileName)
    {
        fileName = null;
        var contentDisposition = response.ContentHeaders?.FirstOrDefault(x => x.Name is "Content-Disposition");

        if (contentDisposition?.Value is not string dispositionValue)
            return false;

        var disposition = new ContentDisposition(dispositionValue);

        if (disposition.FileName == null)
            return false;

        fileName = disposition.FileName;
        return true;
    }

    /// <summary>
    /// Return auth token by key
    /// </summary>
    public static bool TryExtractAuthToken(this string? authKey, [NotNullWhen(true)] out string? token)
    {
        token = null;

        if (string.IsNullOrWhiteSpace(authKey))
            return false;

        var trimmed = authKey.Trim();
        var space = authKey.LastIndexOf(' ');
        token = space == -1 ? trimmed : trimmed[(space + 1)..];
        
        return true;
    }
}