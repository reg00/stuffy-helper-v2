using EnsureThat;

namespace StuffyHelper.Authorization.Core.Features.Friend
{
    public class FriendsRequestShort
    {
        public Guid Id { get; set; }
        public string UserIdFrom { get; set; }
        public string UserIdTo { get; set; } = string.Empty;
        public string UserNameFrom { get; set; } = string.Empty;
        public string UserNameTo { get; set; } = string.Empty;

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
}
