using Moq;
using StuffyHelper.Authorization.Core.Services;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Minio.Features.Storage;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class AvatarServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IAvatarRepository> _avatarRepositoryMoq = new();
        private readonly Mock<IFileStore> _fileStoreMoq = new();

        private AvatarService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new AvatarService(
                _avatarRepositoryMoq.Object,
                _fileStoreMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task DeleteAvatarAsync_EmptyUserId()
        {
            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.DeleteAvatarAsync(string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteAvatarAsync_Success()
        {
            var userId = "test";
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            _avatarRepositoryMoq.Setup(x => x.GetAvatarAsync(userId, CancellationToken))
                .ReturnsAsync(avatar);

            var avatarService = GetService();
            await avatarService.DeleteAvatarAsync(userId, CancellationToken);
        }

        [Fact]
        public async Task GetAvatarAsync_EmptyId()
        {
            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.GetAvatarAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarAsync_Success()
        {
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            _avatarRepositoryMoq.Setup(x => x.GetAvatarAsync(avatar.Id, CancellationToken))
                .ReturnsAsync(avatar);

            var fileStoreMoq = new Mock<IFileStore>();
            fileStoreMoq.Setup(x => x.GetFileAsync(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new MemoryStream(Array.Empty<byte>()));

            var avatarService = GetService();
            var result = await avatarService.GetAvatarAsync(avatar.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetAvatarMetadataAsync_EmptyId()
        {
            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.GetAvatarMetadataAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarMetadataAsync_Success()
        {
            var avatar = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            _avatarRepositoryMoq.Setup(x => x.GetAvatarAsync(avatar.Id, CancellationToken))
                .ReturnsAsync(avatar);

            var avatarService = GetService();
            var result = await avatarService.GetAvatarMetadataAsync(avatar.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task StoreAvatarFormFileAsync_NullModel()
        {
            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.StoreAvatarFormFileAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreAvatarFormFileAsync_NotImage()
        {
            var addEntry = AvatarServiceUnitTestConstants.GetNotImageAddAvatarEntry();

            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.StoreAvatarFormFileAsync(addEntry, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarUri_EmptyId()
        {
            var avatarService = GetService();

            await ThrowsTask(async () => await avatarService.GetAvatarUri(string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetAvatarUri_Success()
        {
            var avatarEntry = AvatarServiceUnitTestConstants.GetCorrectAvatarEntry();

            _avatarRepositoryMoq.Setup(x => x.GetAvatarAsync(avatarEntry.UserId, CancellationToken))
                .ReturnsAsync(avatarEntry);

            _fileStoreMoq.Setup(x => x.ObtainGetPresignedUrl(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("about:blank"));

            var avatarService = GetService();
            var result = await avatarService.GetAvatarUri(avatarEntry.UserId, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
