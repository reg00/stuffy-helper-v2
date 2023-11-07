using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Features.Authorization;
using StuffyHelper.Authorization.Core.Features.Avatar;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Tests.UnitTests.Common;
using System.Security.Claims;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class AuthorizationServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<UserManager<StuffyUser>> userManagerMoq;
        private readonly Mock<RoleManager<IdentityRole>> roleManagerMoq;

        public AuthorizationServiceUnitTests() : base()
        {
            var userNamagerStoreMoq = new Mock<IUserStore<StuffyUser>>();
            userManagerMoq = new Mock<UserManager<StuffyUser>>(userNamagerStoreMoq.Object, null, null, null, null, null, null, null, null);

            roleManagerMoq = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        }

        [Fact]
        public async Task Login_NullModel()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Login(null, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_CannotFindUser()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_EmailNotConfirmed()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(false);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_WrongPassword()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            userManagerMoq.Setup(x =>
            x.CheckPasswordAsync(stuffyUser, loginModel.Password))
                .ReturnsAsync(false);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Login(loginModel, HttpContext), VerifySettings);
        }

        [Fact]
        public async Task Login_Success()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            userManagerMoq.Setup(x =>
            x.CheckPasswordAsync(stuffyUser, loginModel.Password))
                .ReturnsAsync(true);
            userManagerMoq.Setup(x =>
            x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authConfigMoq = new Mock<IOptions<AuthorizationConfiguration>>();
            authConfigMoq.Setup(x =>
            x.Value)
                .Returns(AuthorizationServiceUnitTestConstants.GetCorrectAuthorizationConfiguration());

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                authConfigMoq.Object);

            var result = await authService.Login(loginModel, HttpContext);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Logout_Success()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.Logout(HttpContext);
        }

        [Fact]
        public async Task Register_NullModel()
        {
            var loginModel = AuthorizationServiceUnitTestConstants.GetCorrectLoginModel();

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Register(null), VerifySettings);
        }

        [Fact]
        public async Task Register_UserExists()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Register(registerModel), VerifySettings);
        }

        [Fact]
        public async Task Register_CreateError()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);
            userManagerMoq.Setup(x =>
            x.CreateAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.Register(registerModel), VerifySettings);
        }

        [Fact]
        public async Task Register_Success()
        {
            var registerModel = AuthorizationServiceUnitTestConstants.GetCorrectRegisterModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);
            userManagerMoq.Setup(x =>
            x.CreateAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMoq.Setup(x =>
            x.GenerateEmailConfirmationTokenAsync(It.IsAny<StuffyUser>()))
                .ReturnsAsync("test");

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            var result = await authService.Register(registerModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetRoles_Success()
        {
            roleManagerMoq.Setup(x =>
            x.Roles)
                .Returns(AuthorizationServiceUnitTestConstants.GetCorrectIdentityRoles().AsQueryable());

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            var result = authService.GetRoles();

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task CheckUserIsAdmin_BadClaims()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.CheckUserIsAdmin(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckUserIsAdmin_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsInRoleAsync(stuffyUser, nameof(UserType.Admin)))
                .ReturnsAsync(true);

            var result = await authService.CheckUserIsAdmin(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUserByToken_BadClaims()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.GetUserByToken(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetUserByToken_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsInRoleAsync(stuffyUser, nameof(UserType.Admin)))
                .ReturnsAsync(true);

            var result = await authService.GetUserByToken(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUserLogins_Success()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            userManagerMoq.Setup(x =>
            x.Users)
                .Returns(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUsers().AsQueryable());

            var result = authService.GetUserLogins();

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_EmptyInput()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.DeleteUser(string.Empty, string.Empty), VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_UserNotFound()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.DeleteUser("test", string.Empty), VerifySettings);
        }

        [Fact]
        public async Task DeleteUser_Success()
        {
            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.DeleteUser("test", string.Empty);
        }

        [Fact]
        public async Task UpdateUser_NullModel()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateUser(null, null), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_BadClaims()
        {
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateUser(new ClaimsPrincipal(), updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_UserNotFound()
        {
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateUser(claims, updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_IncorrectUpdate()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.UpdateAsync(It.IsAny<StuffyUser>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateUser(claims, updateModel), VerifySettings);
        }

        [Fact]
        public async Task UpdateUser_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var updateModel = AuthorizationServiceUnitTestConstants.GetCorrectUpdateModel();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.UpdateAsync(It.IsAny<StuffyUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMoq.Setup(x =>
            x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            var result = await authService.UpdateUser(claims, updateModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_BadClaims()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateAvatar(new ClaimsPrincipal(), null), VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_UserNotFound()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.UpdateAvatar(claims, null), VerifySettings);
        }

        [Fact]
        public async Task UpdateAvatar_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var file = AuthorizationServiceUnitTestConstants.CreateTestFormFile();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var avatarServiceMoq = new Mock<IAvatarService>();
            avatarServiceMoq.Setup(x =>
            x.GetAvatarUri(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("about:blank"));

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.UpdateAvatar(claims, file);
        }

        [Fact]
        public async Task RemoveAvatar_BadClaims()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.RemoveAvatar(new ClaimsPrincipal()), VerifySettings);
        }

        [Fact]
        public async Task RemoveAvatar_UserNotFound()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.RemoveAvatar(claims), VerifySettings);
        }

        [Fact]
        public async Task RemoveAvatar_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.RemoveAvatar(claims);
        }

        [Fact]
        public async Task GetUser_EmptyInput()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.GetUser(string.Empty, string.Empty), VerifySettings);
        }

        [Fact]
        public async Task GetUser_UserNotFound()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.GetUser("test", string.Empty), VerifySettings);
        }

        [Fact]
        public async Task GetUser_Success()
        {
            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser());

            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.GetUser("test", string.Empty);
        }

        [Fact]
        public async Task ConfirmEmail_EmptyInput()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ConfirmEmail(string.Empty, string.Empty), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_UserNotFound()
        {
            var authService = new AuthorizationService(
               userManagerMoq.Object,
               roleManagerMoq.Object,
               new Mock<IAvatarService>().Object,
               new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ConfirmEmail("test", "12345"), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_WrongCode()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.ConfirmEmailAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ConfirmEmail("test", "12345"), VerifySettings);
        }

        [Fact]
        public async Task ConfirmEmail_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.ConfirmEmailAsync(It.IsAny<StuffyUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            userManagerMoq.Setup(x =>
            x.GetRolesAsync(stuffyUser))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectRoles());

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            var result = await authService.ConfirmEmail("test", "12345");

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_NullModel()
        {
            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ForgotPasswordAsync(null), VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_UserNotFound()
        {
            var forgotPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectForgotPasswordModel();

            userManagerMoq.Setup(x =>
            x.FindByEmailAsync(forgotPasswordModel.Email))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ForgotPasswordAsync(forgotPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ForgotPasswordAsync_Success()
        {
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();
            var forgotPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectForgotPasswordModel();

            userManagerMoq.Setup(x =>
            x.FindByEmailAsync(forgotPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.IsEmailConfirmedAsync(stuffyUser))
                .ReturnsAsync(true);
            userManagerMoq.Setup(x =>
            x.GeneratePasswordResetTokenAsync(stuffyUser))
                .ReturnsAsync("12345");

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            var result = await authService.ForgotPasswordAsync(forgotPasswordModel);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_NullModel()
        {
            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ResetPasswordAsync(null), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_UserNotFound()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();

            userManagerMoq.Setup(x =>
            x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync((StuffyUser)null);

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ResetPasswordAsync(resetPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_CreateError()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.ResetPasswordAsync(It.IsAny<StuffyUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Failed(AuthorizationServiceUnitTestConstants.GetIdentityErrors())));

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await ThrowsTask(async () => await authService.ResetPasswordAsync(resetPasswordModel), VerifySettings);
        }

        [Fact]
        public async Task ResetPasswordAsync_Success()
        {
            var resetPasswordModel = AuthorizationServiceUnitTestConstants.GetCorrectResetPasswordModel();
            var stuffyUser = AuthorizationServiceUnitTestConstants.GetCorrectStuffyUser();

            userManagerMoq.Setup(x =>
            x.FindByEmailAsync(resetPasswordModel.Email))
                .ReturnsAsync(stuffyUser);
            userManagerMoq.Setup(x =>
            x.ResetPasswordAsync(It.IsAny<StuffyUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            var authService = new AuthorizationService(
                userManagerMoq.Object,
                roleManagerMoq.Object,
                new Mock<IAvatarService>().Object,
                new Mock<IOptions<AuthorizationConfiguration>>().Object);

            await authService.ResetPasswordAsync(resetPasswordModel);
        }
    }
}
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.