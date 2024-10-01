using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UserEntry
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string MiddleName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public Uri? ImageUri { get; init; }

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
