using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Minio.Features.Helpers;

/// <summary>
/// File extensions
/// </summary>
public static class FormFileExtensions
{
    public static bool IsImage(IFormFile file)
    {
        // Get the file's content type
        var contentType = file.ContentType.ToLower();

        // Get the file extension
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        // List of allowed image content types and extensions
        string[] allowedContentTypes = { "image/jpeg", "image/png", "image/gif" };
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        // Check if the content type or extension matches the criteria
        bool isContentTypeValid = Array.IndexOf(allowedContentTypes, contentType) >= 0;
        bool isExtensionValid = Array.IndexOf(allowedExtensions, fileExtension) >= 0;

        // Return true if the content type or extension matches
        return isContentTypeValid || isExtensionValid;
    }
}