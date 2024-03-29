﻿using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseEntry : ITaggableEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; }
        public double Amount { get; set; }
        public bool IsPartial { get; set; }
        public Guid? UnitTypeId { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsComplete { get; set; }

        public virtual List<PurchaseTagEntry> PurchaseTags { get; set; } = new List<PurchaseTagEntry>();
        public virtual UnitTypeEntry? UnitType { get; set; }
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();
        public virtual EventEntry Event { get; set; }
        public virtual ParticipantEntry Owner { get; set; }

        public void PatchFrom(UpdatePurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            IsPartial = entry.IsPartial;
            UnitTypeId = entry.UnitTypeId;
        }
    }
}
