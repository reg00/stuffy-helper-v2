namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for work with reseting paswwords
/// </summary>
public record ResetPasswordResult()
{
    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; init; }
    
    /// <summary>
    /// Confirm reset code
    /// </summary>
    public string Code { get; init; }
}