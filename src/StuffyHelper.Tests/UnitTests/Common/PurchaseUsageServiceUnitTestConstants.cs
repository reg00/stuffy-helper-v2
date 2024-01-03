using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class PurchaseUsageServiceUnitTestConstants
    {
        public static PurchaseUsageEntry GetCorrectPurchaseUsageEntry()
        {
            return new()
            {
                Amount = 1,
                Id = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid(),
                Participant = new()
                {
                    UserId = "123"
                },
                PurchaseId = Guid.NewGuid(),
                Purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry(),
            };
        }

        public static UpsertPurchaseUsageEntry GetCorrectAddPurchaseUsageEntry()
        {
            return new()
            {
                Amount = 5,
                ParticipantId = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
            };
        }

        public static UpsertPurchaseUsageEntry GetCorrectUpdatePurchaseUsageEntry()
        {
            return new()
            {
                Amount = 6,
                ParticipantId = Guid.NewGuid(),
                PurchaseId = Guid.NewGuid(),
            };
        }

        public static PagedData<PurchaseUsageEntry> GetCorrectPurchaseUsageResponse()
        {
            return new()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<PurchaseUsageEntry>()
                {
                    new()
                    {
                        Amount = 1,
                        Id = Guid.NewGuid(),
                        ParticipantId = Guid.NewGuid(),
                        Participant = new()
                        {
                            UserId = "123"
                        },
                        PurchaseId = Guid.NewGuid(),
                    },
                    new()
                    {
                        Amount = 5,
                        Id = Guid.NewGuid(),
                        ParticipantId = Guid.NewGuid(),
                        Participant = new()
                        {
                            UserId = "123"
                        },
                        PurchaseId = Guid.NewGuid(),
                    },
                }
            };
        }
    }
}
