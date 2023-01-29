using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
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
        public virtual DbSet<PurchaseTagEntry> PurchaseTags { get; set; }
        public virtual DbSet<UnitTypeEntry> UnitTypes { get; set; }
        public virtual DbSet<MediaEntry> Medias { get; set; }

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
                entity.Property(e => e.IsPartial).IsRequired().HasDefaultValue(false);
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
                entity.Property(e => e.Amount).HasDefaultValue(1);
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
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
