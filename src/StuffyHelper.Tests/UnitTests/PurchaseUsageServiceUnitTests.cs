using Moq;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Services;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseUsageServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IPurchaseUsageRepository> _purchaseUsageRepositoryMoq = new();

        private PurchaseUsageService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new PurchaseUsageService(
                _purchaseUsageRepositoryMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task GetPurchaseUsageAsync_EmptyInput()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();
            var purchaseUsageService = GetService();

            await ThrowsTask(async () => await purchaseUsageService.GetPurchaseUsageAsync(claims, Guid.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseUsageAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();

            _purchaseUsageRepositoryMoq.Setup(x => x.GetPurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchaseUsage.Id, CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var purchaseUsageService = GetService();
            var result = await purchaseUsageService.GetPurchaseUsageAsync(claims,Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"),  purchaseUsage.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseUsagesAsync_Success()
        {
            var purchaseUsageResponse = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageResponse();

            _purchaseUsageRepositoryMoq.Setup(x =>
            x.GetPurchaseUsagesAsync(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseUsageResponse);

            var purchaseUsageService = GetService();
            var result = await purchaseUsageService.GetPurchaseUsagesAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), 
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = GetService();

            await ThrowsTask(async () => await purchaseUsageService.AddPurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();
            var addPurchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectAddPurchaseUsageEntry();

            _purchaseUsageRepositoryMoq.Setup(x => x.AddPurchaseUsageAsync(It.IsAny<PurchaseUsageEntry>(), CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var purchaseUsageService = GetService();
            var result = await purchaseUsageService.AddPurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), addPurchaseUsage, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = GetService();

            await ThrowsTask(async () => await purchaseUsageService.DeletePurchaseUsageAsync(Guid.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();

            _purchaseUsageRepositoryMoq.Setup(x => x.DeletePurchaseUsageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var purchaseUsageService = GetService();
            await purchaseUsageService.DeletePurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchaseUsage.Id, CancellationToken);

            _purchaseUsageRepositoryMoq.Verify(x => x.DeletePurchaseUsageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = GetService();

            await ThrowsTask(async () => await purchaseUsageService.UpdatePurchaseUsageAsync(Guid.Empty, Guid.Empty,  null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_NotFound()
        {
            var purchaseUsageService = GetService();

            await ThrowsTask(async () => await purchaseUsageService.UpdatePurchaseUsageAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();
            var updatePurchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectUpdatePurchaseUsageEntry();

            _purchaseUsageRepositoryMoq.Setup(x => x.UpdatePurchaseUsageAsync(It.IsAny<PurchaseUsageEntry>(), CancellationToken))
                .ReturnsAsync(purchaseUsage);
            _purchaseUsageRepositoryMoq.Setup(x => x.GetPurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchaseUsage.Id, CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var purchaseUsageService = GetService();
            var result = await purchaseUsageService.UpdatePurchaseUsageAsync(Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), purchaseUsage.Id, updatePurchaseUsage, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
