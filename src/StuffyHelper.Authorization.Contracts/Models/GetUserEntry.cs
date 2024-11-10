namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for get user entry
/// </summary>
public class GetUserEntry
{
    /// <summary>
    /// Identifier
    /// </summary>
    public string Id { get; init; } = string.Empty;
    
    /// <summary>
    /// Username
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; init; } = string.Empty;
    
    /// <summary>
    /// User role
    /// </summary>
    public string Role { get; init; } = string.Empty;
    
    /// <summary>
    /// User first name
    /// </summary>
    public string FirstName { get; init; } = string.Empty;
    
    /// <summary>
    /// User middle name
    /// </summary>
    public string MiddleName { get; init; } = string.Empty;
    
    /// <summary>
    /// User last name
    /// </summary>
    public string LastName { get; init; } = string.Empty;
    
    /// <summary>
    /// User phone
    /// </summary>
    public string Phone { get; init; } = string.Empty;
    
    /// <summary>
    /// Url for avatar image
    /// </summary>
    public Uri? ImageUri { get; init; }
}