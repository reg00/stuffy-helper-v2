using StuffyHelper.Authorization.Core1.Features.Friends;
using StuffyHelper.Authorization.Core1.Models;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class FriendServiceUnitTestConstants
    {
        public static FriendEntry GetCorrectFriendEntry()
        {
            return new("123", "321");
        }

        public static AuthResponse<FriendEntry> GetCorrectAuthResponse()
        {
            return new()
            {
                Total = 1,
                TotalPages = 1,
                Data = new List<FriendEntry>()
                {
                    new FriendEntry("123", "321")
                    {
                        Friend = new ()
                        {
                            Id = "321",
                            UserName = "Тестов Тест Тестович",
                            ImageUri = new Uri("about:blank")
                        }
                    }
                }
            };
        }
    }
}
