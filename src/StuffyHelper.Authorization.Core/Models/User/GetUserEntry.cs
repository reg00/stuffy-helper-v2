namespace StuffyHelper.Authorization.Core.Models.User
{
    public class GetUserEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Uri? ImageUri { get; set; }

        public GetUserEntry()
        { }

        public GetUserEntry(StuffyUser user, IList<string>? roles = null)
        {
            Id = user?.Id;
            Name = user?.UserName;
            Email = user?.Email;
            FirstName = user?.FirstName;
            MiddleName = user?.MiddleName;
            LastName = user?.LastName;
            Phone = user?.PhoneNumber;
            ImageUri = user?.ImageUri;
            Role = roles?.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User);
        }

        public GetUserEntry(UserEntry user)
        {
            Id = user?.Id;
            Name = user?.Name;
            Email = user?.Email;
            FirstName = user?.FirstName;
            MiddleName = user?.MiddleName;
            LastName = user?.LastName;
            Phone = user?.Phone;
            ImageUri = user?.ImageUri;
            Role = user.Role;
        }
    }
}
