using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

/// <summary>
/// Record for forgot password request
/// </summary>
public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}