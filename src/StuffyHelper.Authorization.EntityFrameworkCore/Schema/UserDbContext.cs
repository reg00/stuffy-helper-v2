using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Core.Models.User;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Schema
{
    public class UserDbContext : IdentityDbContext<StuffyUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
