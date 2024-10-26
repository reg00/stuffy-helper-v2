using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;

namespace StuffyHelper.Authorization.Data;

public class DatabaseInitializer : IInitializer
    {
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly UserDbContext _context;
        private readonly IPasswordHasher<StuffyUser> _passwordHasher;

        public DatabaseInitializer(
            ILogger<DatabaseInitializer> logger,
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
                StuffyUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@mail.ru",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@MAIL.RU",
                    FirstName = "Вячеслав",
                    MiddleName = "Олегович",
                    LastName = "Максимов",
                    PhoneNumber = "+79174409895",
                    EmailConfirmed = true
                };

                var existsUser = _context.Users.Where(x => x.UserName == user.UserName).ToList();

                if (existsUser.Any())
                    return;

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

                if (userRole != null)
                {
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = userRole.Id, UserId = user.Id });
                    _context.SaveChanges();
                }

                var adminRole = _context.Roles.FirstOrDefault(x => x.Name == nameof(UserType.Admin));

                if (adminRole != null)
                {
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = adminRole.Id, UserId = user.Id });
                    _context.SaveChanges();
                }

                _logger.LogInformation($"User with name '{user.UserName}' succesfully added");
            }
            catch (Exception)
            {
                // Fire and forget
            }
        }
    }