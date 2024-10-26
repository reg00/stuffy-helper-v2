using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Contracts.Models;

public class UserShortEntry
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public Uri? ImageUri { get; init; }

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