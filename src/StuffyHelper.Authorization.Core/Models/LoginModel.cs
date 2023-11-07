using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Необходимо заполнить логин")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо заполнить пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
