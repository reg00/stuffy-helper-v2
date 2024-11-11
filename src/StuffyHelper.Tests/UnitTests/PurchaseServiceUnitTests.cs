using Moq;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseTag.Pipeline;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IPurchaseStore> _purchaseRepositoryMoq = new();
        private readonly Mock<IPurchaseTagPipeline> _purchaseTagPipelineMoq = new();

        private PurchaseService GetService()
        {
            return new PurchaseService(
                _purchaseRepositoryMoq.Object,
                _purchaseTagPipelineMoq.Object);
        }
        
        [Fact]
        public async Task GetPurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.GetPurchaseAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            var result = await purchaseService.GetPurchaseAsync(purchase.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchasesAsync_Success()
        {
            var purchaseResponse = PurchaseServiceUnitTestConstants.GetCorrectPurchaseResponse();

            _purchaseRepositoryMoq.Setup(x =>
            x.GetPurchasesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<double?>(),
                It.IsAny<double?>(),
                It.IsAny<Guid?>(),
                It.IsAny<IEnumerable<string>?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseResponse);

            var purchaseService = GetService();
            var result = await purchaseService.GetPurchasesAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.AddPurchaseAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            var addPurchase = PurchaseServiceUnitTestConstants.GetCorrectAddPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            var result = await purchaseService.AddPurchaseAsync(addPurchase, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(purchase.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            await purchaseService.DeletePurchaseAsync(purchase.Id, CancellationToken);

            _purchaseRepositoryMoq.Verify(x => x.DeletePurchaseAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task CompletePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(purchase.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            await purchaseService.CompletePurchaseAsync(purchase.Id, CancellationToken);

            _purchaseRepositoryMoq.Verify(x => x.CompletePurchaseAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(purchase.Id, new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            var updatePurchase = PurchaseServiceUnitTestConstants.GetCorrectUpdatePurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.UpdatePurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            var result = await purchaseService.UpdatePurchaseAsync(purchase.Id, updatePurchase, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
