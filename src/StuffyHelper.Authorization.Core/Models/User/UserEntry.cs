using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UserEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Uri? ImageUri { get; set; }

        public UserEntry()
        { }

        public UserEntry(StuffyUser user, IList<string>? roles = null)
        {
            Id = user.Id;
            Name = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Phone = user.PhoneNumber;
            ImageUri = user.ImageUri;
            Role = roles?.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User);
        }
    }
}
