using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseTag;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class PurchaseTagServiceUnitTestConstants
    {
        public static PurchaseTagEntry GetCorrectPurchaseTagEntry()
        {
            return new()
            {
                IsActive = true,
                Id = Guid.NewGuid(),
                Name = "Test",
            };
        }

        public static UpsertPurchaseTagEntry GetCorrectAddPurchaseTagEntry()
        {
            return new()
            {
                Name = "Test",
            };
        }

        public static UpsertPurchaseTagEntry GetCorrectUpdatePurchaseTagEntry()
        {
            return new()
            {
                Name = "Test update",
            };
        }

        public static Response<PurchaseTagEntry> GetCorrectPurchaseTagResponse()
        {
            return new Response<PurchaseTagEntry>()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<PurchaseTagEntry>()
                {
                    new()
                    {
                        IsActive = true,
                        Id = Guid.NewGuid(),
                        Name = "Test",
                    },
                    new()
                    {
                        IsActive = true,
                        Id = Guid.NewGuid(),
                        Name = "Test 2",
                    }
                }
            };
        }
    }
}
