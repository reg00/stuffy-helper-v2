using EnsureThat;

namespace StuffyHelper.Authorization.Core.Features.FriendsRequest
{
    public class FriendsRequestShort
    {
        public Guid Id { get; set; }
        public string UserIdFrom { get; set; }
        public string UserIdTo { get; set; }
        public string UserNameFrom { get; set; }
        public string UserNameTo { get; set; }

        public FriendsRequestShort(FriendsRequest request)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            Id = request.Id;
            UserIdFrom = request.UserIdFrom;
            UserIdTo = request.UserIdTo;
            UserNameFrom = request.UserFrom?.UserName;
            UserNameTo = request.UserTo?.UserName;
        }
    }
}
