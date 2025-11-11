using Moq;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Services;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IPurchaseRepository> _purchaseRepositoryMoq = new();
        private readonly Mock<IPurchaseTagPipeline> _purchaseTagPipelineMoq = new();

        private PurchaseService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new PurchaseService(
                _purchaseRepositoryMoq.Object,
                _purchaseTagPipelineMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task GetPurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.GetPurchaseAsync(Guid.Empty, Guid.Empty,  CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            var result = await purchaseService.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchasesAsync_Success()
        {
            var purchaseResponse = PurchaseServiceUnitTestConstants.GetCorrectPurchaseResponse();

            _purchaseRepositoryMoq.Setup(x =>
            x.GetPurchasesAsync(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<long?>(),
                It.IsAny<long?>(),
                It.IsAny<IEnumerable<string>?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseResponse);

            var purchaseService = GetService();
            var result = await purchaseService.GetPurchasesAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), 
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.AddPurchaseAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            var addPurchase = PurchaseServiceUnitTestConstants.GetCorrectAddPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            var result = await purchaseService.AddPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), addPurchase, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(Guid.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.DeletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            await purchaseService.DeletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken);

            _purchaseRepositoryMoq.Verify(x => x.DeletePurchaseAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task CompletePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(Guid.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.CompletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompletePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            await purchaseService.CompletePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken);

            _purchaseRepositoryMoq.Verify(x => x.CompletePurchaseAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseAsync_EmptyInput()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(Guid.Empty, Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_NotFound()
        {
            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_AlreadyCompleted()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();

            _purchaseRepositoryMoq.Setup(x => x.AddPurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();

            await ThrowsTask(async () => await purchaseService.UpdatePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseAsync_Success()
        {
            var purchase = PurchaseServiceUnitTestConstants.GetCorrectPurchaseEntry();
            var updatePurchase = PurchaseServiceUnitTestConstants.GetCorrectUpdatePurchaseEntry();
            purchase.IsComplete = false;

            _purchaseRepositoryMoq.Setup(x => x.UpdatePurchaseAsync(It.IsAny<PurchaseEntry>(), CancellationToken))
                .ReturnsAsync(purchase);
            _purchaseRepositoryMoq.Setup(x => x.GetPurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, CancellationToken))
                .ReturnsAsync(purchase);

            var purchaseService = GetService();
            var result = await purchaseService.UpdatePurchaseAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchase.Id, updatePurchase, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
