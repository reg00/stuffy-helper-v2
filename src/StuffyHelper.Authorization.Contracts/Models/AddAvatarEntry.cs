using Microsoft.AspNetCore.Http;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Add avatar entry model
/// </summary>
public class AddAvatarEntry
{
    /// <summary>
    /// User id
    /// </summary>
    public string UserId { get; init; } = string.Empty;
    
    /// <summary>
    /// Avatar file
    /// </summary>
    public IFormFile? File { get; init; }
}