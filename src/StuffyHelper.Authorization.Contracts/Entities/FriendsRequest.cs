using System.ComponentModel.DataAnnotations.Schema;
using EnsureThat;

namespace StuffyHelper.Authorization.Contracts.Entities;

public class FriendsRequest
{
    public Guid Id { get; set; }
    public bool IsComfirmed { get; set; }
    public string UserIdFrom { get; set; } = string.Empty;
    public string UserIdTo { get; set; } = string.Empty;

    [ForeignKey("UserIdFrom")]
    public virtual StuffyUser UserFrom { get; init; }
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