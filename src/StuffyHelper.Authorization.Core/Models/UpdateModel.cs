using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле 'UserName'")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; }

        public string? Password { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string? Phone { get; set; }
    }
}
