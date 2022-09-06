using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseType;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.EntityFrameworkCore.Configs;


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
        public virtual DbSet<ShoppingEntry> Shoppings { get; set; }
        public virtual DbSet<PurchaseTypeEntry> PurchaseTypes { get; set; }
        public virtual DbSet<UnitTypeEntry> UnitTypes { get; set; }

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

                entity.HasMany(e => e.Shoppings).WithOne(x => x.Event).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Participants).WithOne(x => x.Event).HasForeignKey(e => e.EventId);

                entity.HasIndex(e => new { e.Name, e.EventDateStart }).IsUnique();

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.EventDateStart).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<ParticipantEntry>(entity =>
            {
                entity.ToTable("participant");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Participants).HasForeignKey(e => e.EventId);
                entity.HasMany(e => e.Shoppings).WithOne(e => e.Participant).HasForeignKey(e => e.ParticipantId);
                entity.HasMany(e => e.PurchaseUsages).WithOne(e => e.Participant).HasForeignKey(e => e.ParticipantId);

                entity.HasIndex(e => new { e.UserId, e.EventId }).IsUnique();

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.EventId).IsRequired();
            });

            modelBuilder.Entity<PurchaseEntry>(entity =>
            {
                entity.ToTable("purchase");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Shopping).WithMany(e => e.Purchases).HasForeignKey(e => e.ShoppingId);
                entity.HasMany(e => e.PurchaseUsages).WithOne(e => e.Purchase).HasForeignKey(e => e.PurchaseId);
                entity.HasOne(e => e.PurchaseType).WithMany(e => e.Purchases).HasForeignKey(e => e.PurchaseTypeId);
                entity.HasOne(e => e.UnitType).WithMany(e => e.Purchases).HasForeignKey(e => e.UnitTypeId);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.ShoppingId).IsRequired();
                entity.Property(e => e.PurchaseTypeId).IsRequired();
                entity.Property(e => e.UnitTypeId).IsRequired();
            });

            modelBuilder.Entity<PurchaseUsageEntry>(entity =>
            {
                entity.ToTable("purchase-usage");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Purchase).WithMany(e => e.PurchaseUsages).HasForeignKey(e => e.PurchaseId);
                entity.HasOne(e => e.Participant).WithMany(e => e.PurchaseUsages).HasForeignKey(e => e.ParticipantId);

                entity.HasIndex(e => new { e.ParticipantId, e.PurchaseId }).IsUnique();

                entity.Property(e => e.PurchaseId).IsRequired();
                entity.Property(e => e.ParticipantId).IsRequired();
            });

            modelBuilder.Entity<ShoppingEntry>(entity =>
            {
                entity.ToTable("shopping");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Event).WithMany(e => e.Shoppings).HasForeignKey(e => e.EventId);
                entity.HasOne(e => e.Participant).WithMany(e => e.Shoppings).HasForeignKey(e => e.ParticipantId);
                entity.HasMany(e => e.Purchases).WithOne(e => e.Shopping).HasForeignKey(e => e.ShoppingId);

                entity.HasIndex(e => e.ShoppingDate);

                entity.Property(e => e.ShoppingDate).IsRequired();
                entity.Property(e => e.ParticipantId).IsRequired();
                entity.Property(e => e.EventId).IsRequired();
            });

            modelBuilder.Entity<PurchaseTypeEntry>(entity =>
            {
                entity.ToTable("purchase-types");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.Purchases).WithOne(e => e.PurchaseType).HasForeignKey(e => e.PurchaseTypeId);

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
