﻿using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.Shopping
{
    public class ShoppingEntry
    {
        public Guid Id { get; set; }
        public DateTime ShoppingDate { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventId { get; set; }
        public string? Check { get; set; }
        public string Description { get; set; }

        public virtual EventEntry Event { get; set; }
        public virtual ParticipantEntry Participant { get; set; }
        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();

        public void PatchFrom(UpdateShoppingEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            ShoppingDate = entry.ShoppingDate;
            ParticipantId = entry.ParticipantId;
            Check = entry.Check;
            Description = entry.Description;
        }

    }
}
