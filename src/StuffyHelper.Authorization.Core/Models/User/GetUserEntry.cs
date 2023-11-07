namespace StuffyHelper.Authorization.Core.Models.User
{
    public class GetUserEntry
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

        public GetUserEntry()
        { }

        public GetUserEntry(UserEntry user)
        {

            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Phone = user.Phone;
            ImageUri = user.ImageUri;
            Role = user.Role;
        }
    }
}
