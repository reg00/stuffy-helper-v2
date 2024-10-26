using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core1.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Необходимо заполнить логин")]
        public string Username { get; init; } = string.Empty;

        [Required(ErrorMessage = "Необходимо заполнить пароль")]
        public string Password { get; init; } = string.Empty;
    }
}
