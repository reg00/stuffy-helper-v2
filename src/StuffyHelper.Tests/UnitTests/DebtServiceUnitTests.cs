using Moq;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Services;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

namespace StuffyHelper.Tests.UnitTests
{
    public class DebtServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IDebtRepository> _debtRepositoryMoq = new();
        private readonly Mock<IEventRepository> _eventRepositoryMoq = new();
        private readonly Mock<ICheckoutRepository> _checkoutRepositoryMoq = new();
        private readonly Mock<IPurchaseUsageRepository> _purchaseUsageRepositoryMoq = new();
        private readonly Mock<IPurchaseService> _purchaseServiceMoq = new();
        private readonly Mock<IAuthorizationClient> _authorizationClientMoq = new();

        private DebtService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();
            
            return new DebtService(
                _debtRepositoryMoq.Object,
                _eventRepositoryMoq.Object,
                _checkoutRepositoryMoq.Object,
                _purchaseUsageRepositoryMoq.Object,
                _purchaseServiceMoq.Object,
                _authorizationClientMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task GetDebtAsync_EmptyInput()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.GetDebtAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            _debtRepositoryMoq.Setup(x => x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);

            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.DebtorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.LenderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecondUserEntry());
            
            var debtService = GetService();
            var result = await debtService.GetDebtAsync(debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetDebtsAsync_ReturnEmpty()
        {
            _debtRepositoryMoq.Setup(x =>
            x.GetDebtsAsync(
                0,
                10,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(DebtServiceUnitTestConstants.GetEmptyDebstResponse());

            var debtService = GetService();
            var result = await debtService.GetDebtsAsync(cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetDebtsAsync_Success()
        {
            var debts = DebtServiceUnitTestConstants.GetCorrectDebstResponse();
            _debtRepositoryMoq.Setup(x =>
            x.GetDebtsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<bool?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(debts);

            foreach (var debt in debts.Data)
            {
                _authorizationClientMoq.Setup(x => x.GetUserById(debt.DebtorId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
                _authorizationClientMoq.Setup(x => x.GetUserById(debt.LenderId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecondUserEntry());
            }
            
            var debtService = GetService();

            var result = await debtService.GetDebtsAsync(0, 10, null, null, null, null, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_EmptyInput()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.SendDebtAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_BadAmount()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.SendDebtAsync(string.Empty, Guid.NewGuid(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_DebtNotFound()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.SendDebtAsync("123", Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task SentDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            _debtRepositoryMoq.Setup(x => x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);
            _debtRepositoryMoq.Setup(x => x.UpdateDebtAsync(It.IsAny<DebtEntry>(), CancellationToken))
                .ReturnsAsync(debtEntry);

            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.DebtorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.LenderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecondUserEntry());
            
            var debtService = GetService();
            var result = await debtService.SendDebtAsync("321", debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_EmptyInput()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.ConfirmDebtAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_DebtNotFound()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.SendDebtAsync("123", Guid.Parse("76a258e7-a85d-44b3-b48f-40c4891ebaa0"), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_NotSendedDebt()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetNotSendedDebtEntry();

            _debtRepositoryMoq.Setup(x => x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);

            var debtService = GetService();

            await ThrowsTask(async () => await debtService.ConfirmDebtAsync("123", debtEntry.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task ConfirmDebtAsync_Success()
        {
            var debtEntry = DebtServiceUnitTestConstants.GetCorrectDebtEntry();

            _debtRepositoryMoq.Setup(x => x.GetDebtAsync(debtEntry.Id, CancellationToken))
                .ReturnsAsync(debtEntry);
            _debtRepositoryMoq.Setup(x => x.UpdateDebtAsync(It.IsAny<DebtEntry>(), CancellationToken))
               .ReturnsAsync(debtEntry);

            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.DebtorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());
            _authorizationClientMoq.Setup(x => x.GetUserById(debtEntry.LenderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectSecondUserEntry());
            
            var debtService = GetService();
            var result = await debtService.ConfirmDebtAsync("123", debtEntry.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_EmptyInput()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.CheckoutEvent(Guid.Empty, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_EventNotFound()
        {
            var debtService = GetService();

            await ThrowsTask(async () => await debtService.CheckoutEvent(Guid.Parse("55a258e7-a85d-44b3-b48f-40c4891ebaa1"), string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CheckoutEvent_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, eventEntry.UserId, CancellationToken))
                .ReturnsAsync(eventEntry);

            _checkoutRepositoryMoq.Setup(x => x.AddCheckoutAsync(It.IsAny<CheckoutEntry>(), CancellationToken))
                .ReturnsAsync(new CheckoutEntry()
                {
                    EventId = eventEntry.Id,
                });

            var debtService = GetService();
            await debtService.CheckoutEvent(eventEntry.Id, eventEntry.UserId, CancellationToken);

            _eventRepositoryMoq.Verify(x => x.GetEventAsync(It.IsAny<Guid>(), It.IsAny<string>(), CancellationToken), Times.Once());
        }
    }
}
