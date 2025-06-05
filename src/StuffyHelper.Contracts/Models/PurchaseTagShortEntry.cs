﻿using EnsureThat;
using System.ComponentModel.DataAnnotations;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class PurchaseTagShortEntry
    {
        [Required] public Guid Id { get; init; }
        [Required] public string Name { get; init; } = string.Empty;

        public PurchaseTagShortEntry()
        {

        }

        public PurchaseTagShortEntry(PurchaseTagEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
        }
    }
}

