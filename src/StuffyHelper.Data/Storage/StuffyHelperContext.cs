using EnsureThat;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Contracts.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace StuffyHelper.Data.Storage
{
    public partial class StuffyHelperContext : DbContext
    {
        public StuffyHelperContext()
        {
            
        }
        public StuffyHelperContext(DbContextOptions<StuffyHelperContext> options) : base(options)
        { }

        public virtual DbSet<EventEntry> Events { get; set; }
        public virtual DbSet<ParticipantEntry> Participants { get; set; }
        public virtual DbSet<PurchaseEntry> Purchases { get; set; }
        public virtual DbSet<PurchaseUsageEntry> PurchaseUsages { get; set; }
        public virtual DbSet<MediaEntry> Medias { get; set; }
        public virtual DbSet<DebtEntry> Debts { get; set; }
        public virtual DbSet<CheckoutEntry> Checkouts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseNpgsql("ConnectionString");
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
            }
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

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.EventId).IsRequired();
                entity.Property(e => e.Cost).IsRequired()
                    .HasAnnotation("Range", new[] { 1, long.MaxValue });
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
                    .HasAnnotation("Range", new[] { 1, long.MaxValue });
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

                entity.Property(e => e.LenderId).IsRequired();
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
