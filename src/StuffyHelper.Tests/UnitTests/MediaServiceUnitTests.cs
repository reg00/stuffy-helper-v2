using Moq;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class MediaServiceUnitTests : UnitTestsBase
    {
        public MediaServiceUnitTests() : base()
        {
            
        }

        [Fact]
        public async Task DeleteMediaAsync_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.DeleteMediaAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteMediaAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                new Mock<IFileStore>().Object);

            await mediaService.DeleteMediaAsync(media.Id, CancellationToken);

            mediaStoreMoq.Verify(x => x.DeleteMediaAsync(media, CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetEventPrimalMediaUri_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.GetEventPrimalMediaUri(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetEventPrimalMediaUri_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetPrimalEventMedia(media.EventId, CancellationToken))
                .ReturnsAsync(media);

            var fileStoreMoq = new Mock<IFileStore>();
            fileStoreMoq.Setup(x =>
            x.ObtainGetPresignedUrl(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new Uri("blank:about"));

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                fileStoreMoq.Object);

            var result = await mediaService.GetEventPrimalMediaUri(media.EventId, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaFormFileAsync_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.GetMediaFormFileAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetMediaFormFileAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            var fileStoreMoq = new Mock<IFileStore>();
            fileStoreMoq.Setup(x =>
            x.GetFileAsync(It.IsAny<string>(), CancellationToken))
                .ReturnsAsync(new MemoryStream());

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                fileStoreMoq.Object);

            var result = await mediaService.GetMediaFormFileAsync(media.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadataAsync_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.GetMediaMetadataAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadataAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetMediaAsync(media.Id, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                new Mock<IFileStore>().Object);

            var result = await mediaService.GetMediaMetadataAsync(media.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPrimalEventMedia_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.GetPrimalEventMedia(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPrimalEventMedia_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectMediaEntry();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetPrimalEventMedia(media.EventId, CancellationToken))
                .ReturnsAsync(media);

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                new Mock<IFileStore>().Object);

            var result = await mediaService.GetPrimalEventMedia(media.EventId, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadatasAsync_EmptyInput()
        {
            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetMediasAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<MediaType?>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new EntityNotFoundException("Media not found"));

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                new Mock<IFileStore>().Object);

            var result = await mediaService.GetMediaMetadatasAsync(0, 10, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetMediaMetadatasAsync_Success()
        {
            var medias = MediaServiceUnitTestConstants.GetCorrectMediaEntries();

            var mediaStoreMoq = new Mock<IMediaStore>();
            mediaStoreMoq.Setup(x =>
            x.GetMediasAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<DateTimeOffset?>(),
                It.IsAny<MediaType?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(medias);

            var mediaService = new MediaService(
                mediaStoreMoq.Object,
                new Mock<IFileStore>().Object);

            var result = await mediaService.GetMediaMetadatasAsync(0, 10, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_EmptyInput()
        {
            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(null, false, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_MediaTypeLinkEmpty()
        {
            var media = MediaServiceUnitTestConstants.GetEmptyLinkMediaEntry();

            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task StoreMediaFormFileAsync_MediaTypeFileEmpty()
        {
            var media = MediaServiceUnitTestConstants.GetEmptyFileMediaEntry();

            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            await ThrowsTask(async () => await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken), VerifySettings);
        }


        [Fact]
        public async Task StoreMediaFormFileAsync_Success()
        {
            var media = MediaServiceUnitTestConstants.GetCorrectAddMediaEntry();

            var mediaService = new MediaService(
                new Mock<IMediaStore>().Object,
                new Mock<IFileStore>().Object);

            var result = await mediaService.StoreMediaFormFileAsync(media, false, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
