using System.ComponentModel.DataAnnotations.Schema;
using EnsureThat;

namespace StuffyHelper.Authorization.Contracts.Entities;

/// <summary>
/// Record for work with friend requests
/// </summary>
public class FriendsRequest
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// If confirmed - users became friends
    /// </summary>
    public bool IsComfirmed { get; set; }
    
    /// <summary>
    /// Initiator user id
    /// </summary>
    public string UserIdFrom { get; set; } = string.Empty;
    
    /// <summary>
    /// To user id
    /// </summary>
    public string UserIdTo { get; set; } = string.Empty;

    
    /// <summary>
    /// Linked initiator user entity
    /// </summary>
    [ForeignKey("UserIdFrom")]
    public virtual StuffyUser UserFrom { get; init; }
    
    /// <summary>
    /// Linked to user entity
    /// </summary>
    [ForeignKey("UserIdTo")]
    public virtual StuffyUser UserTo { get; init; }

    public FriendsRequest(string userIdFrom, string userIdTo)
    {
        EnsureArg.IsNotNullOrWhiteSpace(userIdFrom, nameof(userIdFrom));
        EnsureArg.IsNotNullOrWhiteSpace(userIdTo, nameof(userIdTo));

        UserIdFrom = userIdFrom;
        UserIdTo = userIdTo;
        IsComfirmed = false;
    }
}