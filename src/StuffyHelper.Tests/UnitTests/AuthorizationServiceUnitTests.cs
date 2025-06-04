using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using StuffyHelper.Tests.UnitTests.Common;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Core.Services;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Tests.Common;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class AuthorizationServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<UserManager<StuffyUser>> _userManagerMoq = new(new Mock<IUserStore<StuffyUser>>().Object, null, null, null, null, null, null, null, null);
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMoq = new(new Mock<IRoleStore<IdentityRole>>().Object, Array.Empty<IRoleValidator<IdentityRole>>(), new Mock<ILookupNormalizer>().Object, new Mock<IdentityErrorDescriber>().Object, new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        private readonly Mock<IAvatarService> _avatarServiceMoq = new();
        private readonly Mock<IConfiguration> _configMoq = new();

        private AuthorizationService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            _configMoq.Setup(x => x.GetConfig())
                .Returns(CommonTestConstants.GetCorrectStuffyConfiguration());
            
            return new AuthorizationService(
                _userManagerMoq.Object,
                _roleManagerMoq.Object,
                _avatarServiceMoq.Object,
                _configMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task Login_NullModel()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.Login(null, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_CannotFindUser()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_EmailNotConfirmed()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(false);

            var authService = GetService();

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_WrongPassword()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            _userManagerMoq.Setup(x => x.CheckPasswordAsync(stuffyUser, loginModel.Password))
                .ReturnsAsync(false);

            var authService = GetService();

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_Success()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            _userManagerMoq.Setup(x => x.CheckPasswordAsync(stuffyUser, loginModel.Password))
                .ReturnsAsync(true);
            _userManagerMoq.Setup(x => x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authService = GetService();

            var result = await authService.Login(loginModel, HttpContext);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Register_NullModel()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.Register(null), VerifySettings);
        }

        [Fact]
        public async Task Register_UserExists()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);

            var authService = GetService();

            await ThrowsTask(async () => await authService.Register(registerModel), VerifySettings);
        }

        [Fact]
        public async Task Register_CreateError()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);
            _userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = GetService();

            await ThrowsTask(async () => await authService.Register(registerModel), VerifySettings);
        }

        [Fact]
        public async Task Register_Success()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);
            _userManagerMoq.Setup(x => x.CreateAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMoq.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<StuffyUser>()))
                .ReturnsAsync("test");

            var authService = GetService();
            var result = await authService.Register(registerModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetRoles_Success()
        {
            _roleManagerMoq.Setup(x => x.Roles)
                .Returns(AuthorizationServiceUnitTestConstants.GetCorrectIdentityRoles().AsQueryable());

            var authService = GetService();
            var result = authService.GetRoles();

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task CheckUserIsAdmin_BadClaims()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.CheckUserIsAdmin(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckUserIsAdmin_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            
            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsInRoleAsync(stuffyUser, nameof(UserType.Admin)))
                .ReturnsAsync(true);

            var authService = GetService();
            var result = await authService.CheckUserIsAdmin(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUserByToken_BadClaims()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.GetUserByToken(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetUserByToken_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsInRoleAsync(stuffyUser, nameof(UserType.Admin)))
                .ReturnsAsync(true);
            
            var authService = GetService();
            var result = await authService.GetUserByToken(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUserLogins_Success()
        {
            _userManagerMoq.Setup(x =>
            x.Users)
                .Returns(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().AsQueryable());

            var authService = GetService();
            var result = authService.GetUserLogins();

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_EmptyInput()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.DeleteUser(string.Empty, string.Empty), VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_UserNotFound()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.DeleteUser("test", string.Empty), VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_Success()
        {
            _userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = GetService();

            await authService.DeleteUser("test", string.Empty);
        }

        [Fact]
        public async Task UpdateUser_NullModel()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateUser(null, null), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_BadClaims()
        {
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();

            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateUser(new ClaimsPrincipal(), updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound()
        {
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateUser(claims, updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_IncorrectUpdate()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x =>
            x.UpdateAsync(It.IsAny<StuffyUser>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateUser(claims, updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.UpdateAsync(It.IsAny<StuffyUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMoq.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authService = GetService();
            var result = await authService.UpdateUser(claims, updateModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_BadClaims()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateAvatar(new ClaimsPrincipal(), null), VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_UserNotFound()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.UpdateAvatar(claims, null), VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var file = AuthorizationServiceUnitTestConstants.CreateTestFormFile();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            _avatarServiceMoq.Setup(x =>
            x.GetAvatarUri(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("about:blank"));

            var authService = GetService();

            await authService.UpdateAvatar(claims, file);
        }

        [Fact]
        public async Task RemoveAvatar_BadClaims()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.RemoveAvatar(new ClaimsPrincipal()), VerifySettings);
        }

        [Fact]
        public async Task RemoveAvatar_UserNotFound()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.RemoveAvatar(claims), VerifySettings);
        }

        [Fact]
        public async Task RemoveAvatar_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = GetService();
            await authService.RemoveAvatar(claims);
        }

        [Fact]
        public async Task GetUser_EmptyInput()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.GetUserByName(string.Empty), VerifySettings);
        }

        [Fact]
        public async Task GetUser_UserNotFound()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.GetUserByName("test"), VerifySettings);
        }

        [Fact]
        public async Task GetUser_Success()
        {
            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = GetService();
            await authService.GetUserByName("test");
        }

        [Fact]
        public async Task ConfirmEmail_EmptyInput()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.ConfirmEmail(string.Empty, string.Empty), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_UserNotFound()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.ConfirmEmail("test", "12345"), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_WrongCode()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.ConfirmEmailAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = GetService();

            await ThrowsTask(async () => await authService.ConfirmEmail("test", "12345"), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.ConfirmEmailAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMoq.Setup(x => x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authService = GetService();
            var result = await authService.ConfirmEmail("test", "12345");

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_NullModel()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.ForgotPasswordAsync(null), VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserNotFound()
        {
            var forgotPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectForgotPasswordModel();

            _userManagerMoq.Setup(x => x.FindByEmailAsync(forgotPasswordModel.Email))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.ForgotPasswordAsync(forgotPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var forgotPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectForgotPasswordModel();

            _userManagerMoq.Setup(x => x.FindByEmailAsync(forgotPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            _userManagerMoq.Setup(x => x.GeneratePasswordResetTokenAsync(stuffyUser))
                .ReturnsAsync("12345");

            var authService = GetService();
            var result = await authService.ForgotPasswordAsync(forgotPasswordModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_NullModel()
        {
            var authService = GetService();

            await ThrowsTask(async () => await authService.ResetPasswordAsync(null), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();

            _userManagerMoq.Setup(x => x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync((StuffyUser)null);

            var authService = GetService();

            await ThrowsTask(async () => await authService.ResetPasswordAsync(resetPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_CreateError()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.ResetPasswordAsync(It.IsAny<StuffyUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = GetService();

            await ThrowsTask(async () => await authService.ResetPasswordAsync(resetPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_Success()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            _userManagerMoq.Setup(x => x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            _userManagerMoq.Setup(x => x.ResetPasswordAsync(It.IsAny<StuffyUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var authService = GetService();

            await authService.ResetPasswordAsync(resetPasswordModel);
        }
    }
}
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.