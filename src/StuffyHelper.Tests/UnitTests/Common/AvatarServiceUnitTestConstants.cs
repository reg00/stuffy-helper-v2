using Microsoft.AspNetCore.Http;
using Moq;
using StuffyHelper.Authorization.Core1.Features.Avatar;

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
            file.Setup(x => x.FileName)
                .Returns("test.pdf");

            return new()
            {
                UserId = "test",
                File = file.Object
            };
        }

        public static AddAvatarEntry GetImageAddAvatarEntry()
        {
            var file = new Mock<IFormFile>();
            file.Setup(x =>
            x.ContentType)
                .Returns("image/jpg");
            file.Setup(x => x.FileName)
                .Returns("test.jpg");

            return new()
            {
                UserId = "test",
                File = file.Object
            };
        }
    }
}
