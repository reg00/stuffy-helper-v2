﻿using EnsureThat;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageEntry
    {
        public Guid Id { get; init; }
        public Guid ParticipantId { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid? CheckoutId { get; set; }
        public double Amount { get; set; }

        public virtual ParticipantEntry Participant { get; init; }
        public virtual PurchaseEntry Purchase { get; init; }
        public virtual CheckoutEntry Checkout { get; set; }

        public void PatchFrom(UpsertPurchaseUsageEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            ParticipantId = entry.ParticipantId;
            PurchaseId = entry.PurchaseId;
            Amount = entry.Amount;
        }
    }
}
