using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UpdateModel
    {
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; }

        public string Password { get; set; }

        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить поле имени")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string Phone { get; set; }
    }
}
