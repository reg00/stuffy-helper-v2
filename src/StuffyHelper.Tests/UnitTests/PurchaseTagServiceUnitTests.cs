using Moq;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Services;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseTagServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IPurchaseTagRepository> _purchaseTagRepositoryMoq = new();

        private PurchaseTagService GetService()
        {
            return new PurchaseTagService(_purchaseTagRepositoryMoq.Object);
        }
        
        [Fact]
        public async Task GetPurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = GetService();

            await ThrowsTask(async () => await purchaseTagService.GetPurchaseTagAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();

            _purchaseTagRepositoryMoq.Setup(x => x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = GetService();
            var result = await purchaseTagService.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseTagsAsync_Success()
        {
            var purchaseTagResponse = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagResponse();

            _purchaseTagRepositoryMoq.Setup(x =>
            x.GetPurchaseTagsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseTagResponse);

            var purchaseTagService = GetService();
            var result = await purchaseTagService.GetPurchaseTagsAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = GetService();

            await ThrowsTask(async () => await purchaseTagService.AddPurchaseTagAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();
            var addPurchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectAddPurchaseTagEntry();

            _purchaseTagRepositoryMoq.Setup(x => x.AddPurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = GetService();

            var result = await purchaseTagService.AddPurchaseTagAsync(addPurchaseTag, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = new PurchaseTagService(
               new Mock<IPurchaseTagRepository>().Object);

            await ThrowsTask(async () => await purchaseTagService.DeletePurchaseTagAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();

            _purchaseTagRepositoryMoq.Setup(x => x.AddPurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);
            _purchaseTagRepositoryMoq.Setup(x => x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = GetService();
            await purchaseTagService.DeletePurchaseTagAsync(purchaseTag.Id, CancellationToken);

            _purchaseTagRepositoryMoq.Verify(x => x.DeletePurchaseTagAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_EmptyInput()
        {
            var purchaseTagService = GetService();

            await ThrowsTask(async () => await purchaseTagService.UpdatePurchaseTagAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_NotFound()
        {
            var purchaseTagService = GetService();

            await ThrowsTask(async () => await purchaseTagService.UpdatePurchaseTagAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseTagAsync_Success()
        {
            var purchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectPurchaseTagEntry();
            var updatePurchaseTag = PurchaseTagServiceUnitTestConstants.GetCorrectUpdatePurchaseTagEntry();

            _purchaseTagRepositoryMoq.Setup(x => x.UpdatePurchaseTagAsync(It.IsAny<PurchaseTagEntry>(), CancellationToken))
                .ReturnsAsync(purchaseTag);
            _purchaseTagRepositoryMoq.Setup(x => x.GetPurchaseTagAsync(purchaseTag.Id, CancellationToken))
                .ReturnsAsync(purchaseTag);

            var purchaseTagService = GetService();
            var result = await purchaseTagService.UpdatePurchaseTagAsync(purchaseTag.Id, updatePurchaseTag, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
