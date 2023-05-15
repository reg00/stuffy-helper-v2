using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using StuffyHelper.Api.Controllers;
using StuffyHelper.Authorization.Core.Models;
using System.Security.Claims;

namespace StuffyHelper.Tests.Features.Auth
{
    internal static class AuthMockData
    {
        public static string ConfirmCode = "5782";
        
        public static RegisterModel RegisterModel = new ()
        {
            Email = "test@gmail.com",
            Password = "password",
            Username = "username",
        };

        public static LoginModel LoginModel = new ()
        {
            Username = RegisterModel.Username,
            Password = RegisterModel.Password,
        };
        
        public static ForgotPasswordModel ForgotPasswordModel = new ()
        {
            Email = RegisterModel.Email,
        };

        public static ResetPasswordModel ResetPasswordModel = new ()
        {
            Code = ConfirmCode,
            Email = RegisterModel.Email,
            Password = RegisterModel.Password,
            ConfirmPassword = RegisterModel.Password,
        };

        public static UpdateModel UpdateModel = new ()
        {
            FirstName = "test",
            LastName = "test",
            MiddleName = "test",
            Phone = "test",
            Username = RegisterModel.Username
        };

        public static UserEntry User = new()
        {
            Email = RegisterModel.Email,
            Id = Guid.NewGuid().ToString(),
            Name = RegisterModel.Username,
            Role = "user"
        };

        public static ClaimsPrincipal PrincipalUser = new (new ClaimsIdentity());

        public static FormFile File = new (
            new MemoryStream(),
            1,
            1,
            "test",
            "test");

        public static void SetModelError(this AuthorizationController controller, string key, string error)
        {
            controller.ModelState.AddModelError(key, error);
        }

        public static void SetContextWithUrl(this AuthorizationController controller)
        {
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);

            mockUrlHelper
                .Setup(
                    x => x.Action(
                        It.IsAny<UrlActionContext>()
                    )
                )
                .Returns("callbackUrl")
                .Verifiable();

            request.Setup(x => x.Scheme).Returns("http");
            context.SetupGet(x => x.Request).Returns(request.Object);

            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = context.Object;
        }

        public static void SetContextWithUser(this  AuthorizationController controller)
        {
            var context = new Mock<HttpContext>();
            context.Setup(x => x.User).Returns(PrincipalUser);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = context.Object;
        }

        public static void SetEmptyContext(this AuthorizationController controller)
        {
            var context = new Mock<HttpContext>();

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = context.Object;
        }
    }
}
