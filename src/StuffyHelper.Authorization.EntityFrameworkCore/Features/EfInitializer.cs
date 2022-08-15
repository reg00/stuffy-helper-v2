using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.EntityFrameworkCore.Schema;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features
{
    public class EfInitializer : IInitializer
    {
        private readonly ILogger<EfInitializer> _logger;
        private readonly UserDbContext _context;
        private readonly IPasswordHasher<StuffyUser> _passwordHasher;

        public EfInitializer(
            ILogger<EfInitializer> logger,
            UserDbContext context,
            IPasswordHasher<StuffyUser> passwordHasher)
        {
            EnsureArg.IsNotNull(logger, nameof(logger));
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(passwordHasher, nameof(passwordHasher));

            _logger = logger;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void Initialize()
        {
            InitializeTokenServerConfigurationDatabase();

            _context.SaveChanges();
        }

        private void InitializeTokenServerConfigurationDatabase()
        {
            _context.Database.Migrate();

            RegisterAdmin();
        }

        private void RegisterAdmin()
        {
            try
            {
                var existsUser = _context.Users.Where(x => x.UserName == "admin").ToList();

                if (existsUser.Any())
                    return;

                StuffyUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@rcud-rt.ru",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@RCUD-RT.RU",
                    FirstName = "Вячеслав",
                    MiddleName = "Олегович",
                    LastName = "Максимов",
                    PhoneNumber = "+79174409895"
                };

                _context.Users.Add(user);

                var hashedPassword = _passwordHasher.HashPassword(user, "default");
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.PasswordHash = hashedPassword;

                _context.SaveChanges();

                var roles = _context.Roles.ToList();

                if (roles.FirstOrDefault(x => x.Name == nameof(UserType.Admin)) == null)
                {
                    _context.Roles.Add(
                        new IdentityRole()
                        {
                            Name = nameof(UserType.Admin),
                            NormalizedName = nameof(UserType.Admin).ToUpper()
                        });
                    _context.SaveChanges();
                }

                if (roles.FirstOrDefault(x => x.Name == nameof(UserType.User)) == null)
                {
                    _context.Roles.Add(new IdentityRole()
                    {
                        Name = nameof(UserType.User),
                        NormalizedName = nameof(UserType.User).ToUpper()
                    });
                    _context.SaveChanges();
                }

                var userRole = _context.Roles.FirstOrDefault(x => x.Name == nameof(UserType.User));
                _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = userRole.Id, UserId = user.Id });
                _context.SaveChanges();

                var adminRole = _context.Roles.FirstOrDefault(x => x.Name == nameof(UserType.Admin));
                _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = user.Id });
                _context.SaveChanges();

                _logger.LogInformation($"User with name '{user.UserName}' succesfully added");

                return;
            }
            catch (Exception)
            {
                // Fire and forget
            }
        }
    }
}
