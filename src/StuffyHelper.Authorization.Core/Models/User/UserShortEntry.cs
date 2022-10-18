namespace StuffyHelper.Authorization.Core.Models.User
{
    public class UserShortEntry
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public UserShortEntry()
        { }

        public UserShortEntry(StuffyUser user)
        {
            Id = user?.Id;
            Name = user?.UserName;
        }

        public UserShortEntry(UserEntry user)
        {
            Id = user?.Id;
            Name = user?.Name;
        }
    }
}
