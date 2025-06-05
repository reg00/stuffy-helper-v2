using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Participant;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class ParticipantServiceUnitTestConstants
    {
        public static ParticipantEntry GetCorrectParticipantEntry()
        {
            return new()
            {
                EventId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                UserId = "123",
                Event = EventServiceUnitTestConstants.GetCorrectEventEntry(),
            };
        }

        public static UpsertParticipantEntry GetCorrectUpsertParticipantEntry()
        {
            return new()
            {
                EventId = Guid.NewGuid(),
                UserId = "123"
            };
        }

        public static Response<ParticipantEntry> GetCorrectParticipantResponse()
        {
            return new()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<ParticipantEntry>()
                {
                    new()
                    {
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        UserId = "123"
                    },
                    new()
                    {
                        EventId = Guid.NewGuid(),
                        Id = Guid.NewGuid(),
                        UserId = "123"
                    }
                }
            };
        }
    }
}
