using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле логина")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        [Required(ErrorMessage = "Необходимо заполнить поле электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле пароля")]
        public string Password { get; set; }

        public UserType UserType { get; set; }
    }
}
