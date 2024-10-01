using Moq;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class PurchaseUsageServiceUnitTests : UnitTestsBase
    {
        [Fact]
        public async Task GetPurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = new PurchaseUsageService(
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await purchaseUsageService.GetPurchaseUsageAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();

            var purchaseUsageStoreMoq = new Mock<IPurchaseUsageStore>();
            purchaseUsageStoreMoq.Setup(x =>
            x.GetPurchaseUsageAsync(purchaseUsage.Id, CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(purchaseUsage.Participant.UserId))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var purchaseUsageService = new PurchaseUsageService(
                purchaseUsageStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await purchaseUsageService.GetPurchaseUsageAsync(purchaseUsage.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetPurchaseUsagesAsync_Success()
        {
            var purchaseUsageResponse = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageResponse();

            var purchaseUsageStoreMoq = new Mock<IPurchaseUsageStore>();
            purchaseUsageStoreMoq.Setup(x =>
            x.GetPurchaseUsagesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(purchaseUsageResponse);

            var purchaseUsageService = new PurchaseUsageService(
                purchaseUsageStoreMoq.Object,
                new Mock<IAuthorizationService>().Object);

            var result = await purchaseUsageService.GetPurchaseUsagesAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = new PurchaseUsageService(
               new Mock<IPurchaseUsageStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await purchaseUsageService.AddPurchaseUsageAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddPurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();
            var addPurchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectAddPurchaseUsageEntry();

            var purchaseUsageStoreMoq = new Mock<IPurchaseUsageStore>();
            purchaseUsageStoreMoq.Setup(x =>
            x.AddPurchaseUsageAsync(It.IsAny<PurchaseUsageEntry>(), CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var purchaseUsageService = new PurchaseUsageService(
                purchaseUsageStoreMoq.Object,
               new Mock<IAuthorizationService>().Object);

            var result = await purchaseUsageService.AddPurchaseUsageAsync(addPurchaseUsage, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = new PurchaseUsageService(
               new Mock<IPurchaseUsageStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await purchaseUsageService.DeletePurchaseUsageAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();

            var purchaseUsageStoreMoq = new Mock<IPurchaseUsageStore>();
            purchaseUsageStoreMoq.Setup(x =>
            x.DeletePurchaseUsageAsync(It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var purchaseUsageService = new PurchaseUsageService(
               purchaseUsageStoreMoq.Object,
               new Mock<IAuthorizationService>().Object);

            await purchaseUsageService.DeletePurchaseUsageAsync(purchaseUsage.Id, CancellationToken);

            purchaseUsageStoreMoq.Verify(x => x.DeletePurchaseUsageAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_EmptyInput()
        {
            var purchaseUsageService = new PurchaseUsageService(
               new Mock<IPurchaseUsageStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await purchaseUsageService.UpdatePurchaseUsageAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_NotFound()
        {
            var purchaseUsageService = new PurchaseUsageService(
               new Mock<IPurchaseUsageStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await purchaseUsageService.UpdatePurchaseUsageAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePurchaseUsageAsync_Success()
        {
            var purchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectPurchaseUsageEntry();
            var updatePurchaseUsage = PurchaseUsageServiceUnitTestConstants.GetCorrectUpdatePurchaseUsageEntry();

            var purchaseUsageStoreMoq = new Mock<IPurchaseUsageStore>();
            purchaseUsageStoreMoq.Setup(x =>
            x.UpdatePurchaseUsageAsync(It.IsAny<PurchaseUsageEntry>(), CancellationToken))
                .ReturnsAsync(purchaseUsage);
            purchaseUsageStoreMoq.Setup(x =>
            x.GetPurchaseUsageAsync(purchaseUsage.Id, CancellationToken))
                .ReturnsAsync(purchaseUsage);

            var purchaseUsageService = new PurchaseUsageService(
                purchaseUsageStoreMoq.Object,
                new Mock<IAuthorizationService>().Object);

            var result = await purchaseUsageService.UpdatePurchaseUsageAsync(purchaseUsage.Id, updatePurchaseUsage, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
