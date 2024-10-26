using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Contracts.Models;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;
}