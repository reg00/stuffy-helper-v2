using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class PurchaseServiceUnitTestConstants
    {
        public static PurchaseEntry GetCorrectPurchaseEntry()
        {
            return new()
            {
                Cost = 1,
                EventId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                IsComplete = true,
                Name = "Test",
                ParticipantId = Guid.NewGuid(),
                Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
            };
        }

        public static AddPurchaseEntry GetCorrectAddPurchaseEntry()
        {
            return new()
            {
                Cost = 1,
                Name = "Test",
                ParticipantId = Guid.NewGuid(),
            };
        }

        public static UpdatePurchaseEntry GetCorrectUpdatePurchaseEntry()
        {
            return new()
            {
                Cost = 3,
                Name = "Test",
            };
        }

        public static Response<PurchaseEntry> GetCorrectPurchaseResponse()
        {
            return new()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<PurchaseEntry>()
                {
                    new()
                    {
                        Cost = 1,
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        IsComplete = true,
                        Name = "Test",
                        ParticipantId = Guid.NewGuid(),
                        Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                        Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
                    },
                    new()
                    {
                        Cost = 5,
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        IsComplete = true,
                        Name = "Test 2",
                        ParticipantId = Guid.NewGuid(),
                        Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                        Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
                    }
                }
            };
        }
    }
}
