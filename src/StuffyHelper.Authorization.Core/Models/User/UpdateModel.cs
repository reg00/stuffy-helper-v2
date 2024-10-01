using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UpdateModel
    {
        [Required(ErrorMessage = "Необходимо заполнить поле 'UserName'")]
        public string Username { get; init; } = string.Empty;

        public string FirstName { get; init; } = string.Empty;

        public string MiddleName { get; init; } = string.Empty;

        public string LastName { get; init; } = string.Empty;

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string Phone { get; init; } = string.Empty;
    }
}
