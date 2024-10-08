﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace StuffyHelper.Tests.Common
{
    public static class CommonTestConstants
    {
        public static CancellationToken GetCancellationToken() => new CancellationToken();

        public static HttpContext GetHttpContext()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            authServiceMock
                .Setup(s => s.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));
            authServiceMock
                .Setup(s => s.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(p => p.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = serviceProviderMock.Object
            };

            return httpContext;
        }
    }
}
