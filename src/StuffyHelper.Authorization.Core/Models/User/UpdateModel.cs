using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле 'UserName'")]
        public string Username { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string Phone { get; set; } = string.Empty;
    }
}
