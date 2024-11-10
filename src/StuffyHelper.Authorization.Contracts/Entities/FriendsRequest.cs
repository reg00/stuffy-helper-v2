using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Authorization.Contracts.Entities;

/// <summary>
/// Record for work with friend requests
/// </summary>
public class FriendsRequest
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// If confirmed - users became friends
    /// </summary>
    public bool IsComfirmed { get; set; }
    
    /// <summary>
    /// Initiator user id
    /// </summary>
    public string UserIdFrom { get; init; } = string.Empty;
    
    /// <summary>
    /// To user id
    /// </summary>
    public string UserIdTo { get; init; } = string.Empty;
    
    /// <summary>
    /// Linked initiator user entity
    /// </summary>
    [ForeignKey("UserIdFrom")]
    public StuffyUser UserFrom { get; init; }
    
    /// <summary>
    /// Linked to user entity
    /// </summary>
    [ForeignKey("UserIdTo")]
    public StuffyUser UserTo { get; init; }
}