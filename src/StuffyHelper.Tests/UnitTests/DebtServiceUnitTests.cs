using Moq;
using StuffyHelper.Authorization.Core1.Features;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Tests.UnitTests.Common;

namespace StuffyHelper.Tests.UnitTests
{
    public class DebtServiceUnitTests : UnitTestsBase
    {
        [Fact]
        public async Task GetDebtAsync_EmptyInput()
        {
            var debtService = new DebtService(
                new Mock<IDebtStore>().Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.GetDebtAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("123"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("321"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecontUserEntry());

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IPurchaseService>().Object);

            var result = await debtService.GetDebtAsync(debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetDebtsAsync_ReturnEmpty()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtsAsync(
                0,
                10,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(DebtServiceUnitTestConstants.GetEmptyDebstResponse());

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            var result = await debtService.GetDebtsAsync(cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetDebtsAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtsAsync(
                0,
                10,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(DebtServiceUnitTestConstants.GetCorrectDebstResponse());

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("123"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("321"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecontUserEntry());

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IPurchaseService>().Object);

            var result = await debtService.GetDebtsAsync(cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_EmptyInput()
        {
            var debtService = new DebtService(
                new Mock<IDebtStore>().Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.SendDebtAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_BadAmount()
        {
            var debtService = new DebtService(
                new Mock<IDebtStore>().Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.SendDebtAsync(string.Empty, Guid.NewGuid(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_DebtNotFound()
        {
            var debtService = new DebtService(
                new Mock<IDebtStore>().Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.SendDebtAsync("123", Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);
            debtStore.Setup(x =>
            x.UpdateDebtAsync(It.IsAny<DebtEntry>(), CancellationToken))
                .ReturnsAsync(debtEntry);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("123"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("321"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecontUserEntry());

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IPurchaseService>().Object);

            var result = await debtService.SendDebtAsync("321", debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_EmptyInput()
        {
            var debtService = new DebtService(
               new Mock<IDebtStore>().Object,
               new Mock<IEventStore>().Object,
               new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
               new Mock<IAuthorizationService>().Object,
               new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.ConfirmDebtAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_DebtNotFound()
        {
            var debtService = new DebtService(
                new Mock<IDebtStore>().Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.SendDebtAsync("123", Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_NotSendedDebt()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetNotSendedDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.ConfirmDebtAsync("123", debtEntry.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            var debtStore = new Mock<IDebtStore>();
            debtStore.Setup(x =>
            x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);
            debtStore.Setup(x =>
           x.UpdateDebtAsync(It.IsAny<DebtEntry>(), CancellationToken))
               .ReturnsAsync(debtEntry);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("123"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            authorizationServiceMoq.Setup(x =>
            x.GetUserById("321"))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecontUserEntry());

            var debtService = new DebtService(
                debtStore.Object,
                new Mock<IEventStore>().Object,
                new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IPurchaseService>().Object);

            var result = await debtService.ConfirmDebtAsync("123", debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_EmptyInput()
        {
            var debtService = new DebtService(
              new Mock<IDebtStore>().Object,
              new Mock<IEventStore>().Object,
              new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
              new Mock<IAuthorizationService>().Object,
              new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.CheckoutEvent(Guid.Empty, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_EventNotFound()
        {
            var debtService = new DebtService(
              new Mock<IDebtStore>().Object,
              new Mock<IEventStore>().Object,
              new Mock<ICheckoutStore>().Object,
                new Mock<IPurchaseUsageStore>().Object,
              new Mock<IAuthorizationService>().Object,
              new Mock<IPurchaseService>().Object);

            await ThrowsTask(async () => await debtService.CheckoutEvent(Guid.Parse("55a258e7-a85d-44b3-b48f-40c4891ebaa1"), string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            var eventStoreMoq = new Mock<IEventStore>();
            eventStoreMoq.Setup(x =>
            x.GetEventAsync(eventEntry.Id, eventEntry.UserId, CancellationToken))
                .ReturnsAsync(eventEntry);

            var checkoutStoreMoq = new Mock<ICheckoutStore>();
            checkoutStoreMoq.Setup(x =>
            x.AddCheckoutAsync(It.IsAny<CheckoutEntry>(), CancellationToken))
                .ReturnsAsync(new CheckoutEntry()
                {
                    EventId = eventEntry.Id,
                });

            var debtService = new DebtService(
              new Mock<IDebtStore>().Object,
              eventStoreMoq.Object,
              checkoutStoreMoq.Object,
              new Mock<IPurchaseUsageStore>().Object,
              new Mock<IAuthorizationService>().Object,
              new Mock<IPurchaseService>().Object);

            await debtService.CheckoutEvent(eventEntry.Id, eventEntry.UserId, CancellationToken);

            eventStoreMoq.Verify(x => x.GetEventAsync(It.IsAny<Guid>(), It.IsAny<string>(), CancellationToken), Times.Once());
        }
    }
}
