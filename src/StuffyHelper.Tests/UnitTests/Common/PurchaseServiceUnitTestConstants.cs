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
                Amount = 1,
                Cost = 1,
                EventId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                IsComplete = true,
                IsPartial = true,
                Name = "Test",
                ParticipantId = Guid.NewGuid(),
                UnitTypeId = Guid.NewGuid(),
                Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                UnitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry(),
                Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
            };
        }

        public static AddPurchaseEntry GetCorrectAddPurchaseEntry()
        {
            return new()
            {
                Amount = 1,
                Cost = 1,
                IsPartial = true,
                Name = "Test",
                ParticipantId = Guid.NewGuid(),
                UnitTypeId = Guid.NewGuid(),
            };
        }

        public static UpdatePurchaseEntry GetCorrectUpdatePurchaseEntry()
        {
            return new()
            {
                Amount = 3,
                Cost = 3,
                IsPartial = false,
                Name = "Test",
                UnitTypeId = Guid.NewGuid(),
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
                        Amount = 1,
                        Cost = 1,
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        IsComplete = true,
                        IsPartial = true,
                        Name = "Test",
                        ParticipantId = Guid.NewGuid(),
                        UnitTypeId = Guid.NewGuid(),
                        Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                        UnitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry(),
                        Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
                    },
                    new()
                    {
                        Amount = 5,
                        Cost = 5,
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        IsComplete = true,
                        IsPartial = true,
                        Name = "Test 2",
                        ParticipantId = Guid.NewGuid(),
                        UnitTypeId = Guid.NewGuid(),
                        Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
                        UnitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry(),
                        Owner = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry(),
                    }
                }
            };
        }
    }
}
