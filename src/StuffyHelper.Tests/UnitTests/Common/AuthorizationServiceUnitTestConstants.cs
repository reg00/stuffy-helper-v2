using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Contracts;

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

        public static GetUserEntry GetCorrectUserEntry()
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
        
        public static GetUserEntry GetCorrectSecondUserEntry()
        {
            return new()
            {
                FirstName = "Второв",
                MiddleName = "Вторович",
                LastName = "Второв",
                Id = "651",
                Name = "vtor"
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

        public static StuffyClaims GetCorrectStuffyClaims()
        {
            return new StuffyClaims()
            {
                UserId = "test",
                Roles = new List<string>() { "admin", "user" },
                Username = "test"
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
