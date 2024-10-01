namespace StuffyHelper.Authorization.Core.Models.User
{
    public class GetUserEntry
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
