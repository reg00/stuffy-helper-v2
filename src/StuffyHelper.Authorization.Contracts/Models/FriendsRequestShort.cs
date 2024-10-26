using EnsureThat;
using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Contracts.Models;

public class FriendsRequestShort
{
    public Guid Id { get; init; }
    public string UserIdFrom { get; init; }
    public string UserIdTo { get; init; } = string.Empty;
    public string? UserNameFrom { get; init; } = string.Empty;
    public string? UserNameTo { get; init; } = string.Empty;

    public FriendsRequestShort(FriendsRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        Id = request.Id;
        UserIdFrom = request.UserIdFrom;
        UserIdTo = request.UserIdTo;
        UserNameFrom = request.UserFrom.UserName;
        UserNameTo = request.UserTo.UserName;
    }
}