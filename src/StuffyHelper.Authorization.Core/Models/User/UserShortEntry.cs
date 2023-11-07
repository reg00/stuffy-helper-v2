namespace StuffyHelper.Authorization.Core.Models.User
{
    public class UserShortEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Uri? ImageUri { get; set; }

        public UserShortEntry()
        { }

        public UserShortEntry(StuffyUser user)
        {
            Id = user.Id;
            Name = user.UserName;
            ImageUri = user.ImageUri;
        }

        public UserShortEntry(UserEntry user)
        {
            Id = user.Id;
            Name = user.Name;
            ImageUri = user.ImageUri;
        }
    }
}
