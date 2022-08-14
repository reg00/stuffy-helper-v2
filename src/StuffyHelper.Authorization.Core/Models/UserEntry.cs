using Microsoft.AspNetCore.Identity;

namespace StuffyHelper.Authorization.Core.Models
{
    public class UserEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public UserEntry()
        { }

        public UserEntry(IdentityUser user, IList<string> roles = null)
        {
            Id = user?.Id;
            Name = user?.UserName;
            Email = user?.Email;
            Role = roles?.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User);
        }
    }
}
