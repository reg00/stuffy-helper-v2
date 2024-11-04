using EnsureThat;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.Entities;

/// <summary>
/// Record of stuffy user (depends on identity user)
/// </summary>
public class StuffyUser : IdentityUser
{
    /// <summary>
    /// User first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// User middle name
    /// </summary>
    public string MiddleName { get; set; } = string.Empty;
    
    /// <summary>
    /// User last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// User avatar url
    /// </summary>
    public Uri? ImageUri { get; set; }

    /// <summary>
    /// Linked list of incoming friend requests 
    /// </summary>
    public virtual List<FriendsRequest> IncomingRequests { get; set; } = new List<FriendsRequest>();
    
    /// <summary>
    /// Linked list of sended friend requests 
    /// </summary>
    public virtual List<FriendsRequest> SendedRequests { get; set; } = new List<FriendsRequest>();
    
    /// <summary>
    /// Linked list of user friends
    /// </summary>
    public virtual List<FriendEntry> Friends { get; set; } = new List<FriendEntry>();
    
    /// <summary>
    /// Linked entity of avatar
    /// </summary>
    public virtual AvatarEntry? Avatar { get; set; }

    public void PatchFrom(UpdateModel model)
    {
        EnsureArg.IsNotNull(model, nameof(model));

        FirstName = model.FirstName;
        MiddleName = model.MiddleName;
        LastName = model.LastName;
        UserName = model.Username;
        PhoneNumber = model.Phone;
    }
}