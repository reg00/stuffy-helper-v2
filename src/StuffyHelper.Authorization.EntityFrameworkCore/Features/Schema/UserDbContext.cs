using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Core.Features.FriendsRequest;
using StuffyHelper.Authorization.Core.Models.Friends;
using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema
{
    public class UserDbContext : IdentityDbContext<StuffyUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public virtual DbSet<AspNetFriends> Friends { get; set; }
        public virtual DbSet<FriendsRequest> FriendsRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetFriends>(entity =>
            {
                entity.ToTable("asp-net-friends");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserAId).IsRequired();
                entity.Property(e => e.UserBId).IsRequired();
                entity.Property(e => e.FriendsSince).IsRequired();
            });

            modelBuilder.Entity<FriendsRequest>(entity =>
            {
                entity.ToTable("friends-requests");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.UserFrom).WithMany(e => e.SendedRequests).HasForeignKey(e => e.UserIdFrom);
                entity.HasOne(e => e.UserTo).WithMany(e => e.IncomingRequests).HasForeignKey(e => e.UserIdTo);

                entity.Property(e => e.UserIdFrom).IsRequired();
                entity.Property(e => e.UserIdTo).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
