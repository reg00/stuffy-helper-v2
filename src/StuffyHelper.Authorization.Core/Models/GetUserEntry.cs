using Microsoft.AspNetCore.Identity;

namespace StuffyHelper.Authorization.Core.Models
{
    public class GetUserEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public GetUserEntry()
        { }

        public GetUserEntry(IdentityUser user, IList<string> roles = null)
        {
            Id = user?.Id;
            Name = user?.UserName;
            Email = user?.Email;
            Role = roles?.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User);
        }

        public GetUserEntry(UserEntry user)
        {
            Id = user?.Id;
            Name = user?.Name;
            Email = user?.Email;
            Role = user.Role;
        }
    }
}
