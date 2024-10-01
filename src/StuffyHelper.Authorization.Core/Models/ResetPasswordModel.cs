using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Code { get; init; } = string.Empty;
    }
}
