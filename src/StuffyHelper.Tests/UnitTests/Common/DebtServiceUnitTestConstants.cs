using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class DebtServiceUnitTestConstants
    {
        public static DebtEntry GetCorrectDebtEntry()
        {
            return new()
            {
                Amount = 1,
                LenderId = "123",
                DebtorId = "321",
                EventId = Guid.NewGuid(),
                Id = Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"),
                IsComfirmed = false,
                IsSent = true,
                Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
            };
        }

        public static DebtEntry GetNotSendedDebtEntry()
        {
            return new()
            {
                Amount = 1,
                LenderId = "123",
                DebtorId = "321",
                EventId = Guid.NewGuid(),
                Id = Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"),
                IsComfirmed = false,
                IsSent = false
            };
        }

        public static Response<DebtEntry> GetEmptyDebstResponse()
        {
            return new()
            {
                Data = new List<DebtEntry>(),
                Total = 0,
                TotalPages = 0
            };
        }

        public static Response<DebtEntry> GetCorrectDebstResponse()
        {
            return new()
            {
                Data = new List<DebtEntry>()
                {
                    new()
                    {
                        Amount = 1,
                        LenderId = "123",
                        DebtorId = "321",
                        Event = new ()
                        {
                            IsActive = true,
                            Description = "test",
                            Name = "test",
                        }
                    },
                    new()
                    {
                        Amount = 2,
                        LenderId = "321",
                        DebtorId = "123",
                        Event = new ()
                        {
                            IsActive = true,
                            Description = "test 2",
                            Name = "test 2",
                        }
                    },
                },
                Total = 2,
                TotalPages = 1
            };
        }
    }
}
