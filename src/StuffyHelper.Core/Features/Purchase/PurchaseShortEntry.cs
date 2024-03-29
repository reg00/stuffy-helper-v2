﻿using EnsureThat;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.UnitType;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public bool IsPartial { get; set; }
        [Required]
        public bool IsComplete { get; set; }

        public List<PurchaseTagShortEntry> PurchaseTags { get; set; }
        public UnitTypeShortEntry? UnitType { get; set; }

        public PurchaseShortEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            IsPartial = entry.IsPartial;
            IsComplete = entry.IsComplete;
            UnitType = entry.UnitType == null ? null : new UnitTypeShortEntry(entry.UnitType);
            PurchaseTags = entry.PurchaseTags.Select(x => new PurchaseTagShortEntry(x)).ToList();
        }
    }
}
