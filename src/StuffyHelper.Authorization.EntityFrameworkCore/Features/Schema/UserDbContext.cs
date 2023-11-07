using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Core.Features.Friends;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Authorization.Core.Features.Avatar;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema
{
    public class UserDbContext : IdentityDbContext<StuffyUser>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public virtual DbSet<FriendEntry> Friends { get; set; }
        public virtual DbSet<FriendsRequest> FriendsRequests { get; set; }
        public virtual DbSet<AvatarEntry> Avatars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendEntry>(entity =>
            {
                entity.ToTable("friends");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.FriendId });

                entity.HasOne(e => e.User).WithMany(e => e.Friends).HasForeignKey(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.FriendId).IsRequired();
                entity.Property(e => e.FriendsSince).IsRequired();
            });

            modelBuilder.Entity<FriendsRequest>(entity =>
            {
                entity.ToTable("friends-requests");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserIdFrom, e.UserIdTo });

                entity.HasOne(e => e.UserFrom).WithMany(e => e.SendedRequests).HasForeignKey(e => e.UserIdFrom);
                entity.HasOne(e => e.UserTo).WithMany(e => e.IncomingRequests).HasForeignKey(e => e.UserIdTo);

                entity.Property(e => e.UserIdFrom).IsRequired();
                entity.Property(e => e.UserIdTo).IsRequired();
            });

            modelBuilder.Entity<AvatarEntry>(entity =>
            {
                entity.ToTable("avatars");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId).IsUnique();

                entity.HasOne(e => e.User).WithOne(e => e.Avatar).HasForeignKey<AvatarEntry>(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
