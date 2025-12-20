using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class FriendsRequestUnitTestConstants
    {
        public static FriendsRequest GetCorrectRequest()
        {
            return new()
            {
                UserIdFrom = "123",
                UserIdTo = "321",
                UserFrom = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().First(),
                UserTo = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().Last(),
            };
        }

        public static IEnumerable<FriendsRequest> GetCorrectRequests()
        {
            return new List<FriendsRequest>()
            {
                new()
                {
                    UserIdFrom = "123",
                    UserIdTo = "321",
                },
                new()
                {
                    UserIdFrom = "321",
                    UserIdTo = "222",
                },
                new()
                {
                    UserIdFrom = "222",
                    UserIdTo = "126",
                },
            };
        }
    }
}
