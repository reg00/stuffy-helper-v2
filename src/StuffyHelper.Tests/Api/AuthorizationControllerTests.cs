using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;
using StuffyHelper.Api.Controllers;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Configs;
using StuffyHelper.EmailService.Core.Service;
using StuffyHelper.Tests.Features.Auth;

namespace StuffyHelper.Tests.Api
{
    public class AuthorizationControllerTests
    {
        private AuthorizationController _controller;

        [SetUp]
        public void Setup()
        {
            var authorizationServiceMock = new Mock<IAuthorizationService>();
            authorizationServiceMock.Setup(x => x.Register(AuthMockData.RegisterModel)).ReturnsAsync(AuthMockData.ConfirmCode);
            authorizationServiceMock.Setup(x => x.ConfirmEmail(string.Empty, string.Empty)).ThrowsAsync(new ArgumentNullException("code"));
            authorizationServiceMock.Setup(x => x.ConfirmEmail(AuthMockData.RegisterModel.Username, AuthMockData.ConfirmCode)).ReturnsAsync(AuthMockData.User);
            authorizationServiceMock.Setup(x => x.CheckUserIsAdmin(AuthMockData.PrincipalUser, default)).ReturnsAsync(true);
            authorizationServiceMock.Setup(x => x.GetUserByToken(AuthMockData.PrincipalUser, default)).ReturnsAsync(AuthMockData.User);
            authorizationServiceMock.Setup(x => x.UpdateUser(AuthMockData.PrincipalUser, AuthMockData.UpdateModel)).ReturnsAsync(AuthMockData.User);

            var emailService = new Mock<IEmailService>();
            emailService.Setup(x => x.SendEmailAsync(AuthMockData.RegisterModel.Email, AuthMockData.RegisterModel.Username, "test")).Returns(Task.CompletedTask);

            var frontEndOptions = Options.Create(new FrontEndConfiguration { Endpoint = new Uri("https://testapi.com") });

            _controller = new AuthorizationController(authorizationServiceMock.Object, emailService.Object, frontEndOptions);
        }

        [Test]
        public async Task Register_InvalidModel_Test()
        {            
            _controller.SetModelError("email", "Error while validating email");
            
            var result = await _controller.Register(AuthMockData.RegisterModel);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void Register_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.Register(null));
        }

        [Test]
        public async Task Register_Success_Test()
        {
            _controller.SetContextWithUrl();

            var result = await _controller.Register(AuthMockData.RegisterModel);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ComfirmEmail_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.ConfirmEmail(string.Empty, string.Empty));
        }

        [Test]
        public async Task ComfirmEmail_Success_Test()
        {
            var result = await _controller.ConfirmEmail(AuthMockData.RegisterModel.Username, AuthMockData.ConfirmCode);
            var redirectResult = result as RedirectResult;

            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.EqualTo("https://testapi.com/register/success"));
        }

        [Test]
        public async Task Login_InvalidModel_Test()
        {
            _controller.SetModelError("username", "Username is required");

            var result = await _controller.Login(AuthMockData.LoginModel);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void Login_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.Login(null));
        }

        [Test]
        public async Task Logout_Success_Test()
        {
            var result = await _controller.Logout();
            var okResult = result as OkResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task ForgotPassword_InvalidModel_Test()
        {
            _controller.SetModelError("email", "Error while validating email");

            var result = await _controller.ForgotPassword(AuthMockData.ForgotPasswordModel);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void ForgotPassword_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.ForgotPassword(null));
        }

        [Test]
        public async Task ForgotPassword_Success_Test()
        {
            _controller.SetContextWithUrl();

            var result = await _controller.ForgotPassword(AuthMockData.ForgotPasswordModel);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ResetPassword_Get_NullInputValue_Test()
        {
            Assert.Throws<ArgumentException>(() => _controller.ResetPassword(string.Empty));
        }

        [Test]
        public void ResetPassword_Get_Success_Test()
        {
            var result = _controller.ResetPassword(AuthMockData.ConfirmCode);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task ResetPassword_Post_InvalidModel_Test()
        {
            _controller.SetModelError("email", "Error while validating email");

            var result = await _controller.ResetPassword(AuthMockData.ResetPasswordModel);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void ResetPassword_Post_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.ResetPassword((ResetPasswordModel)null));
        }

        [Test]
        public void ResetPassword_Post_Success_Test()
        {
            var result = _controller.ResetPassword(AuthMockData.ConfirmCode);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetRoles_Success_Test()
        {
            _controller.SetContextWithUser();

            var result = await _controller.GetRoles();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetRoles_Forbid_Test()
        {
           _controller.SetEmptyContext();

            var result = await _controller.GetRoles();
            var okResult = result as ForbidResult;

            Assert.IsNotNull(okResult);
        }

        [Test]
        public async Task CheckUserIsAdmin_Admin_Test()
        {
            _controller.SetContextWithUser();

            var result = await _controller.CheckUserIsAdmin();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That((bool)okResult.Value!, Is.EqualTo(true));
        }

        [Test]
        public async Task CheckUserIsAdmin_User_Test()
        {
            _controller.SetEmptyContext();

            var result = await _controller.CheckUserIsAdmin();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That((bool)okResult.Value!, Is.EqualTo(false));
        }

        [Test]
        public void GetAccountInfoAsync_NoAuth_Test()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.GetAccountInfoAsync());
        }

        [Test]
        public async Task GetAccountInfoAsync_Success_Test()
        {
            _controller.SetContextWithUser();

            var result = await _controller.GetAccountInfoAsync();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetUserLoginsAsync_Success_Test()
        {
            var result = await _controller.GetUserLogins();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task EditUserAsync_InvalidModel_Test()
        {
            _controller.SetModelError("email", "Error while validating email");

            var result = await _controller.EditUserAsync(AuthMockData.UpdateModel);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void EditUserAsync_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.EditUserAsync(null));
        }

        [Test]
        public async Task EditUserAsync_Success_Test()
        {
            _controller.SetContextWithUser();

            var result = await _controller.EditUserAsync(AuthMockData.UpdateModel);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void EditAvatarAsync_NullInputValue_Test()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.EditAvatarAsync(null));
        }

        [Test]
        public async Task EditAvatarAsync_Success_Test()
        {
            var result = await _controller.EditAvatarAsync(AuthMockData.File);
            var okResult = result as OkResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task RemoveAvatarAsync_Success_Test()
        {
            var result = await _controller.RemoveAvatarAsync();
            var okResult = result as OkResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
        }
    }
}
