using StuffyHelper.Authorization.Core1.Features.Friend;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class FriendsRequestUnitTestConstants
    {
        public static FriendsRequest GetCorrectRequest()
        {
            return new("123", "321")
            {
                UserFrom = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().First(),
                UserTo = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().Last(),
            };
        }

        public static IEnumerable<FriendsRequest> GetCorrectRequests()
        {
            return new List<FriendsRequest>()
            {
                new("123", "321"),
                new("321", "222"),
                new("222", "126"),
            };
        }
    }
}
