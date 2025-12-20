using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class FriendServiceUnitTestConstants
    {
        public static FriendEntry GetCorrectFriendEntry()
        {
            return new()
            {
                UserId = "123",
                FriendId = "321"
            };
        }

        public static Response<FriendEntry> GetCorrectAuthResponse()
        {
            return new()
            {
                Total = 1,
                TotalPages = 1,
                Data = new List<FriendEntry>()
                {
                    new()
                    {
                        UserId = "123",
                        FriendId = "321",
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
