using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core1.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле логина")]
        public string Username { get; init; } = string.Empty;

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        [Required(ErrorMessage = "Необходимо заполнить поле электронной почты")]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Необходимо заполнить поле пароля")]
        public string Password { get; init; } = string.Empty;
    }
}
