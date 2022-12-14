using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UserEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; }

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
            Role = roles?.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User);
        }
    }
}
