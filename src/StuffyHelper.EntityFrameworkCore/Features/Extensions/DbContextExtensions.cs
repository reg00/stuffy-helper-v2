using Microsoft.EntityFrameworkCore;
using StuffyHelper.EntityFrameworkCore.Configs;
using StuffyHelper.EntityFrameworkCore.Features.Exceptions;

namespace StuffyHelper.EntityFrameworkCore.Features.Extensions
{
    public static class DbContextExtensions
    {
        public static void UseDatabase(this DbContextOptionsBuilder optionsBuilder,
            EntityFrameworkConfiguration configuration)
        {
            // Add db providers to config if you want to use another ones
            if (configuration.DbProvider.Equals(Constants.PostgreSqlProviderName, StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.UseNpgsql(configuration.ConnectionString);
            }
            else
            {
                throw new InvalidDbProviderException();
            }
        }
    }
}
