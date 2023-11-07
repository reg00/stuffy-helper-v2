using Microsoft.AspNetCore.Http;
using Moq;
using StuffyHelper.Authorization.Core.Features.Avatar;

namespace StuffyHelper.Tests.UnitTests.Common
{
    public static class AvatarServiceUnitTestConstants
    {
        public static AvatarEntry GetCorrectAvatarEntry()
        {
            return new()
            {
                FileName = "testfile",
                FileType = Minio.Features.Common.FileType.Jpg,
                Id = Guid.NewGuid(),
                UserId = "testuser"
            };
        }

        public static AddAvatarEntry GetNotImageAddAvatarEntry()
        {
            var file = new Mock<IFormFile>();
            file.Setup(x =>
            x.ContentType)
                .Returns("application/pdf");

            return new()
            {
                UserId = "test",
                File = file.Object
            };
        }
    }
}
