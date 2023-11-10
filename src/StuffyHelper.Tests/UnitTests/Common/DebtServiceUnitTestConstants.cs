using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class DebtServiceUnitTestConstants
    {
        public static DebtEntry GetCorrectDebtEntry()
        {
            return new()
            {
                Amount = 1,
                BorrowerId = "123",
                DebtorId = "321",
                EventId = Guid.NewGuid(),
                Id = Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"),
                IsComfirmed = false,
                IsSent = true,
                Paid = 1
            };
        }

        public static DebtEntry GetNotSendedDebtEntry()
        {
            return new()
            {
                Amount = 1,
                BorrowerId = "123",
                DebtorId = "321",
                EventId = Guid.NewGuid(),
                Id = Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"),
                IsComfirmed = false,
                IsSent = false,
                Paid = 1
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
                        BorrowerId = "123",
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
                        BorrowerId = "321",
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

                    }
                }
            };
        }
    }
}
