using Moq;
using StuffyHelper.Authorization.Core.Features.Avatar;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class AvatarServiceUnitTests : UnitTestsBase
    {
        public AvatarServiceUnitTests() : base()
        {

        }

        [Fact]
        public async Task DeleteAvatarAsync_EmptyUserId()
        {
            var avatarService = new AvatarService(
                new Mock<IAvatarStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.DeleteAvatarAsync(string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteAvatarAsync_Success()
        {
            var userId = "test";
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            var avatarStoreMoq = new Mock<IAvatarStore>();
            avatarStoreMoq.Setup(x =>
            x.GetAvatarAsync(userId, CancellationToken))
                .ReturnsAsync(avatar);

            var avatarService = new AvatarService(
                avatarStoreMoq.Object,
                new Mock<IFileStore>().Object);

            await avatarService.DeleteAvatarAsync(userId, CancellationToken);
        }

        [Fact]
        public async Task GetAvatarAsync_EmptyId()
        {
            var avatarService = new AvatarService(
                new Mock<IAvatarStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.GetAvatarAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarAsync_Success()
        {
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            var avatarStoreMoq = new Mock<IAvatarStore>();
            avatarStoreMoq.Setup(x =>
            x.GetAvatarAsync(avatar.Id, CancellationToken))
                .ReturnsAsync(avatar);

            var fileStoreMoq = new Mock<IFileStore>();
            fileStoreMoq.Setup(x =>
            x.GetFileAsync(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new MemoryStream(new byte[0]));

            var avatarService = new AvatarService(
                avatarStoreMoq.Object,
                fileStoreMoq.Object);

            var result = await avatarService.GetAvatarAsync(avatar.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetAvatarMetadataAsync_EmptyId()
        {
            var avatarService = new AvatarService(
                new Mock<IAvatarStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.GetAvatarMetadataAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarMetadataAsync_Success()
        {
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            var avatarStoreMoq = new Mock<IAvatarStore>();
            avatarStoreMoq.Setup(x =>
            x.GetAvatarAsync(avatar.Id, CancellationToken))
                .ReturnsAsync(avatar);

            var avatarService = new AvatarService(
                avatarStoreMoq.Object,
                new Mock<IFileStore>().Object);

            var result = await avatarService.GetAvatarMetadataAsync(avatar.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task StoreAvatarFormFileAsync_NullModel()
        {
            var avatarService = new AvatarService(
               new Mock<IAvatarStore>().Object,
               new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.StoreAvatarFormFileAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreAvatarFormFileAsync_NotImage()
        {
            var addEntry = AvatarServiceUnitTestConstants.GetNotImageAddAvatarEntry();

            var avatarService = new AvatarService(
               new Mock<IAvatarStore>().Object,
               new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.StoreAvatarFormFileAsync(addEntry, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarUri_EmptyId()
        {
            var avatarService = new AvatarService(
                new Mock<IAvatarStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await avatarService.GetAvatarUri(string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarUri_Success()
        {
            var avatarEntry = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            var avatarStoreMoq = new Mock<IAvatarStore>();
            avatarStoreMoq.Setup(x =>
            x.GetAvatarAsync(avatarEntry.UserId, CancellationToken))
                .ReturnsAsync(avatarEntry);

            var fileStoreMoq = new Mock<IFileStore>();
            fileStoreMoq.Setup(x =>
            x.ObtainGetPresignedUrl(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("about:blank"));

            var avatarService = new AvatarService(
                avatarStoreMoq.Object,
                fileStoreMoq.Object);

            var result = await avatarService.GetAvatarUri(avatarEntry.UserId, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
