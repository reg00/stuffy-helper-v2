using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class EventServiceUnitTestConstants
    {
        public static AddEventEntry GetCorrectAddEventEntry()
        {
            return new()
            {
                Description = "description",
                Name = "name",
            };
        }

        public static UpdateEventEntry GetCorrectUpdateEventEntry()
        {
            return new()
            {
                Description = "description updated",
                Name = "name updated",
            };
        }

        public static EventEntry GetCorrectEventEntry()
        {
            return new()
            {
                IsActive = true,
                Description = "test",
                Id = Guid.NewGuid(),
                IsCompleted = true,
                Name = "test",
                UserId = "123",
                Purchases = new List<PurchaseEntry>()
                {
                    new()
                    {
                        Amount = 1,
                        Cost = 1,
                        IsPartial = true,
                        EventId = Guid.NewGuid(),
                        Name = "test",
                        IsComplete = false,
                        ParticipantId = Guid.NewGuid(),
                        PurchaseUsages = new List<PurchaseUsageEntry>()
                        {
                            new()
                            {
                                Amount = 1,
                                Id = Guid.NewGuid(),
                                ParticipantId= Guid.NewGuid(),
                                PurchaseId = Guid.NewGuid(),
                                Participant = new()
                                {
                                    UserId = "666"
                                }
                            }
                        },
                        Owner = new()
                        {
                            UserId = "555"
                        },
                        UnitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry()
                    }
                }
            };
        }

        public static Response<EventEntry> GetCorrectEventsResponse()
        {
            return new()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<EventEntry>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                    }
                }
            };
        }
    }
}
