using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
