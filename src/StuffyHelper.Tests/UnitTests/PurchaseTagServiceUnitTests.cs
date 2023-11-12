using Moq;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseTagServiceUnitTests : UnitTestsBase
    {
        public PurchaseTagServiceUnitTests() : base()
        { }

        [Fact]
        public async Task GetPurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = new PurchaseTagService(
                new Mock<IPurchaseTagStore>().Object);

            await ThrowsTask(async () => await purchaseTagService.GetPurchaseTagAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();

            var purchaseTagStoreMoq = new Mock<IPurchaseTagStore>();
            purchaseTagStoreMoq.Setup(x =>
            x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = new PurchaseTagService(
                purchaseTagStoreMoq.Object);

            var result = await purchaseTagService.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseTagsAsync_Success()
        {
            var purchaseTagResponse = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagResponse();

            var purchaseTagStoreMoq = new Mock<IPurchaseTagStore>();
            purchaseTagStoreMoq.Setup(x =>
            x.GetPurchaseTagsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseTagResponse);

            var purchaseTagService = new PurchaseTagService(
                purchaseTagStoreMoq.Object);

            var result = await purchaseTagService.GetPurchaseTagsAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = new PurchaseTagService(
               new Mock<IPurchaseTagStore>().Object);

            await ThrowsTask(async () => await purchaseTagService.AddPurchaseTagAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();
            var addPurchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectAddPurchaseTagEntry();

            var purchaseTagStoreMoq = new Mock<IPurchaseTagStore>();
            purchaseTagStoreMoq.Setup(x =>
            x.AddPurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = new PurchaseTagService(
                purchaseTagStoreMoq.Object);

            var result = await purchaseTagService.AddPurchaseTagAsync(addPurchaseTag, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = new PurchaseTagService(
               new Mock<IPurchaseTagStore>().Object);

            await ThrowsTask(async () => await purchaseTagService.DeletePurchaseTagAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();

            var purchaseTagStoreMoq = new Mock<IPurchaseTagStore>();
            purchaseTagStoreMoq.Setup(x =>
            x.AddPurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);
            purchaseTagStoreMoq.Setup(x =>
            x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = new PurchaseTagService(
               purchaseTagStoreMoq.Object);

            await purchaseTagService.DeletePurchaseTagAsync(purchaseTag.Id, CancellationToken);

            purchaseTagStoreMoq.Verify(x => x.DeletePurchaseTagAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = new PurchaseTagService(
               new Mock<IPurchaseTagStore>().Object);

            await ThrowsTask(async () => await purchaseTagService.UpdatePurchaseTagAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_NotFound()
        {
            var purchaseTagService = new PurchaseTagService(
               new Mock<IPurchaseTagStore>().Object);

            await ThrowsTask(async () => await purchaseTagService.UpdatePurchaseTagAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();
            var updatePurchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectUpdatePurchaseTagEntry();

            var purchaseTagStoreMoq = new Mock<IPurchaseTagStore>();
            purchaseTagStoreMoq.Setup(x =>
            x.UpdatePurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);
            purchaseTagStoreMoq.Setup(x =>
            x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = new PurchaseTagService(
                purchaseTagStoreMoq.Object);

            var result = await purchaseTagService.UpdatePurchaseTagAsync(purchaseTag.Id, updatePurchaseTag, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
