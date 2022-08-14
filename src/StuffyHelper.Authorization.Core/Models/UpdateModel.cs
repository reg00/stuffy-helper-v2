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
    }
}
