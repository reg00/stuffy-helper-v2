using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Core.Features.Friends;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema
{
    public class UserDbContext : IdentityDbContext<StuffyUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public virtual DbSet<FriendEntry> Friends { get; set; }
        public virtual DbSet<FriendsRequest> FriendsRequests { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
