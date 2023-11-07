using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле логина")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        [Required(ErrorMessage = "Необходимо заполнить поле электронной почты")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо заполнить поле пароля")]
        public string Password { get; set; } = string.Empty;
    }
}
