using Moq;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class MediaServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IMediaRepository> _mediaRepositoryMoq = new();
        private readonly Mock<IFileStore> _fileRepositoryMoq = new();

        private MediaService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();
            
            return new MediaService(
                _mediaRepositoryMoq.Object,
                _fileRepositoryMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task DeleteMediaAsync_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.DeleteMediaAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteMediaAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            _mediaRepositoryMoq.Setup(x => x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = GetService();
            await mediaService.DeleteMediaAsync(media.Id, CancellationToken);

            _mediaRepositoryMoq.Verify(x => x.DeleteMediaAsync(media, CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetEventPrimalMediaUri_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.GetEventPrimalMediaUri(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetEventPrimalMediaUri_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            _mediaRepositoryMoq.Setup(x => x.GetPrimalEventMedia(media.EventId, CancellationToken))
                .ReturnsAsync(media);

            _fileRepositoryMoq.Setup(x => x.ObtainGetPresignedUrl(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("blank:about"));

            var mediaService = GetService();
            var result = await mediaService.GetEventPrimalMediaUri(media.EventId, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaFormFileAsync_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.GetMediaFormFileAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetMediaFormFileAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            _mediaRepositoryMoq.Setup(x => x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            _fileRepositoryMoq.Setup(x => x.GetFileAsync(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new MemoryStream());

            var mediaService = GetService();

            var result = await mediaService.GetMediaFormFileAsync(media.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadataAsync_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.GetMediaMetadataAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadataAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            _mediaRepositoryMoq.Setup(x => x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = GetService();
            var result = await mediaService.GetMediaMetadataAsync(media.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPrimalEventMedia_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.GetPrimalEventMedia(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPrimalEventMedia_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            _mediaRepositoryMoq.Setup(x => x.GetPrimalEventMedia(media.EventId, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = GetService();
            var result = await mediaService.GetPrimalEventMedia(media.EventId, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadatasAsync_EmptyInput()
        {
            _mediaRepositoryMoq.Setup(x =>
            x.GetMediasAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<MediaType?>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new EntityNotFoundException("Media not found"));

            var mediaService = GetService();
            var result = await mediaService.GetMediaMetadatasAsync(0, 10, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadatasAsync_Success()
        {
            var medias = MediaServiceUnitTestConstants.GetCorrectMediaEntries();

            _mediaRepositoryMoq.Setup(x =>
            x.GetMediasAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<MediaType?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(medias);

            var mediaService = GetService();
            var result = await mediaService.GetMediaMetadatasAsync(0, 10, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_EmptyInput()
        {
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(null, false, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_MediaTypeLinkEmpty()
        {
            var media = MediaServiceUnitTestConstants.GetEmptyLinkMediaEntry();
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_MediaTypeFileEmpty()
        {
            var media = MediaServiceUnitTestConstants.GetEmptyFileMediaEntry();
            var mediaService = GetService();

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken), VerifySettings);
        }


        [Fact]
        public async Task StoreMediaFormFileAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectAddMediaEntry();
            var mediaService = GetService();
            var result = await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
