using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class UnitTypeServiceUnitTestConstants
    {
        public static UnitTypeEntry GetCorrectUnitTypeEntry()
        {
            return new()
            {
                IsActive = true,
                Id = Guid.NewGuid(),
                Name = "Test",
            };
        }

        public static UpsertUnitTypeEntry GetCorrectAddUnitTypeEntry()
        {
            return new()
            {
                Name = "Test",
            };
        }

        public static UpsertUnitTypeEntry GetCorrectUpdateUnitTypeEntry()
        {
            return new()
            {
                Name = "Test updated",
            };
        }

        public static Response<UnitTypeEntry> GetCorrectUnitTypeResponse()
        {
            return new()
            {
                Total = 2,
                TotalPages = 1,
                Data = new List<UnitTypeEntry>()
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
