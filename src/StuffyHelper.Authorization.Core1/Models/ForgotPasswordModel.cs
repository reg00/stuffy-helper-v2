using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core1.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
    }
}
