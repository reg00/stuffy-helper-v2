using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.EntityFrameworkCore.Configs;
using StuffyHelper.EntityFrameworkCore.Features.Common;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace StuffyHelper.EntityFrameworkCore.Features.Schema
{
    public partial class StuffyHelperContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        private readonly EntityFrameworkConfiguration _configuration;

        public StuffyHelperContext(IOptions<EntityFrameworkConfiguration> configuration)
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            _configuration = configuration.Value;
        }

        public virtual DbSet<EventEntry> Events { get; set; }
        public virtual DbSet<ParticipantEntry> Participants { get; set; }
        public virtual DbSet<PurchaseEntry> Purchases { get; set; }
        public virtual DbSet<PurchaseUsageEntry> PurchaseUsages { get; set; }
        public virtual DbSet<PurchaseTagEntry> PurchaseTags { get; set; }
        public virtual DbSet<UnitTypeEntry> UnitTypes { get; set; }
        public virtual DbSet<MediaEntry> Medias { get; set; }
        public virtual DbSet<DebtEntry> Debts { get; set; }
        public virtual DbSet<CheckoutEntry> Checkouts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            EnsureArg.IsNotNull(optionsBuilder, nameof(optionsBuilder));

            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql(_configuration.ConnectionString);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EnsureArg.IsNotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<EventEntry>(entity =>
            {
                entity.ToTable("event");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Purchases).WithOne(x => x.Event).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Participants).WithOne(x => x.Event).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Medias).WithOne(x => x.Event).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Checkouts).WithOne(x => x.Event).HasForeignKey(e => e.EventId);

                entity.HasIndex(e => new { e.Name, e.EventDateStart }).IsUnique();

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.EventDateStart).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
                entity.Property(e => e.ImageUri).IsRequired(false);
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.EventDateEnd).IsRequired(false);
            });

            modelBuilder.Entity<ParticipantEntry>(entity =>
            {
                entity.ToTable("participant");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Participants).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Purchases).WithOne(e => e.Owner).HasForeignKey(e => e.ParticipantId);
                entity.HasMany(e => e.PurchaseUsages).WithOne(e => e.Participant).HasForeignKey(e => e.ParticipantId);

                entity.HasIndex(e => new { e.UserId, e.EventId }).IsUnique();

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.EventId).IsRequired();
            });

            modelBuilder.Entity<PurchaseEntry>(entity =>
            {
                entity.ToTable("purchase");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Purchases).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.PurchaseUsages).WithOne(e => e.Purchase).HasForeignKey(e => e.PurchaseId);
                entity.HasOne(e => e.UnitType).WithMany(e => e.Purchases).HasForeignKey(e => e.UnitTypeId);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.EventId).IsRequired();
                entity.Property(e => e.UnitTypeId).IsRequired();
                entity.Property(e => e.Amount).IsRequired()
                    .HasAnnotation("Range", new[] { 0.01, double.MaxValue })
                    .HasPrecision(18, 2)
                    .HasConversion(v => Math.Ceiling(v * 100) / 100, v => v);
                entity.Property(e => e.Cost).IsRequired()
                    .HasAnnotation("Range", new[] { 0.01, double.MaxValue })
                    .HasPrecision(18, 2)
                    .HasConversion(v => Math.Ceiling(v * 100) / 100, v => v);
                entity.Property(e => e.IsPartial).IsRequired().HasDefaultValue(false);
                entity.Property(e => e.IsComplete).IsRequired().HasDefaultValue(false);
            });

            modelBuilder.Entity<PurchaseUsageEntry>(entity =>
            {
                entity.ToTable("purchase-usage");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Purchase).WithMany(e => e.PurchaseUsages).HasForeignKey(e => e.PurchaseId);
                entity.HasOne(e => e.Participant).WithMany(e => e.PurchaseUsages).HasForeignKey(e => e.ParticipantId);
                entity.HasOne(e => e.Checkout).WithMany(e => e.PurchaseUsages).HasForeignKey(e => e.CheckoutId);

                entity.HasIndex(e => new { e.ParticipantId, e.PurchaseId }).IsUnique();

                entity.Property(e => e.CheckoutId).IsRequired(false);
                entity.Property(e => e.PurchaseId).IsRequired();
                entity.Property(e => e.ParticipantId).IsRequired();
                entity.Property(e => e.Amount).IsRequired()
                    .HasPrecision(18, 2)
                    .HasConversion(v => Math.Ceiling(v * 100) / 100, v => v)
                    .HasAnnotation("Range", new[] { 0.01, double.MaxValue });
            });

            modelBuilder.Entity<PurchaseTagEntry>(entity =>
            {
                entity.ToTable("purchase-tags");
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<UnitTypeEntry>(entity =>
            {
                entity.ToTable("unit-types");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Purchases).WithOne(e => e.UnitType).HasForeignKey(e => e.UnitTypeId);
                entity.HasIndex(e => e.Name).IsUnique();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<MediaEntry>(entity =>
            {
                entity.ToTable("media");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Medias).HasForeignKey(e => e.EventId);

                entity.Property(e => e.EventId).IsRequired();
                entity.Property(e => e.MediaType).IsRequired();
                entity.Property(e => e.Link).IsRequired(false);
                entity.Property(e => e.FileName).IsRequired(false);
            });

            modelBuilder.Entity<DebtEntry>(entity =>
            {
                entity.ToTable("debts");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Debts).HasForeignKey(e => e.EventId);
                entity.HasOne(e => e.Checkout).WithMany(e => e.Debts).HasForeignKey(e => e.CheckoutId);

                entity.Property(e => e.BorrowerId).IsRequired();
                entity.Property(e => e.DebtorId).IsRequired();
                entity.Property(e => e.IsComfirmed).HasDefaultValue(false);
                entity.Property(e => e.IsSent).HasDefaultValue(false);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.CheckoutId).IsRequired(false);
            });

            modelBuilder.Entity<CheckoutEntry>(entity =>
            {
                entity.ToTable("checkouts");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Checkouts).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.PurchaseUsages).WithOne(e => e.Checkout).HasForeignKey(e => e.CheckoutId);
                entity.HasMany(e => e.Debts).WithOne(e => e.Checkout).HasForeignKey(e => e.CheckoutId);

                entity.HasIndex(e => e.EventId);

                entity.Property(e => e.EventId).IsRequired();
            });

            SeedData(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnitTypeEntry>().HasData(SeedHelper.GetSeedUnitTypes());
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
