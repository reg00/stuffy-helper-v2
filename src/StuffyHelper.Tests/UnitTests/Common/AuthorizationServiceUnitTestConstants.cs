using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using System.Security.Claims;
using System.Text;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class AuthorizationServiceUnitTestConstants
    {
        public static LoginModel GetCorrectLoginModel()
        {
            return new()
            {
                Username = "guest",
                Password = "guest"
            };
        }

        public static UpdateModel GetCorrectUpdateModel()
        {
            return new()
            {
                FirstName = "Оттест",
                LastName = "Оттестов",
                MiddleName = "Оттестович",
                Phone = "1234567899",
                Username = "test"
            };
        }

        public static ForgotPasswordModel GetCorrectForgotPasswordModel()
        {
            return new()
            {
                Email = "test@mail.ru"
            };
        }

        public static ResetPasswordModel GetCorrectResetPasswordModel()
        {
            return new()
            {
                Code = "123",
                ConfirmPassword = "123",
                Email = "test@mail.ru",
                Password = "password"
            };
        }

        public static UserEntry GetCorrectUserEntry()
        {
            return new()
            {
                FirstName = "Тест",
                MiddleName = "Тестович",
                LastName = "Тестов",
                Id = "123",
                Name = "test"
            };
        }

        public static UserEntry GetCorrectSecontUserEntry()
        {
            return new()
            {
                FirstName = "Тест",
                MiddleName = "Тестович",
                LastName = "Тестов",
                Id = "321",
            };
        }

        public static StuffyUser GetCorrectStuffyUser()
        {
            return new()
            {
                FirstName = "Тест",
                MiddleName = "Тестович",
                LastName = "Тестов",
                UserName = "TTTest"
            };
        }

        public static IList<string> GetCorrectRoles()
        {
            return new List<string>()
            {
                nameof(UserType.User),
                nameof(UserType.Admin)
            };
        }

        public static IList<IdentityRole> GetCorrectIdentityRoles()
        {
            return new List<IdentityRole>()
            {
                new()
                {
                    Id = "1",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new()
                {
                    Id = "2",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };
        }

        public static AuthorizationConfiguration GetCorrectAuthorizationConfiguration()
        {
            return new()
            {
                ConnectionString = "connectionString",
                JWT = new()
                {
                    ValidAudience = "audience",
                    Secret = "secret",
                    TokenExpireInHours = 1,
                    ValidIssuer = "issuer"
                }
            };
        }

        public static RegisterModel GetCorrectRegisterModel()
        {
            return new()
            {
                Email = "test@mail.ru",
                Password = "password",
                Username = "TTTest"
            };
        }

        public static IdentityError[] GetIdentityErrors()
        {
            return new IdentityError[]
            {
                new()
                {
                    Code = "1",
                    Description = "error 1"
                },
                new()
                {
                    Code = "2",
                    Description = "error 2"
                }
            };
        }

        public static ClaimsPrincipal GetCorrectClaims()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, "test"),
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic"));
        }

        public static List<StuffyUser> GetCorrectStuffyUsers()
        {
            return new()
            {
                new()
                {
                    UserName = "user1"
                },
                new()
                {
                    UserName = "user2"
                },
                new()
                {
                    UserName = "user3"
                },
            };
        }

        public static IFormFile CreateTestFormFile()
        {
            byte[] bytes = Encoding.UTF8.GetBytes("test data to file");

            return new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "test"
            );
        }
    }
}
