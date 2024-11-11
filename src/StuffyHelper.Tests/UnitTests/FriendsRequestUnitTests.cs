using Moq;
using StuffyHelper.Tests.UnitTests.Common;
using System.Security.Claims;
using StuffyHelper.Authorization.Core.Services;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class FriendsRequestUnitTests : UnitTestsBase
    {
        private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMoq = new();
        private readonly Mock<IAuthorizationService> _authorizationServiceMoq = new();
        private readonly Mock<IFriendService> _friendServiceMoq = new();

        private FriendsRequestService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new FriendsRequestService(
                _friendRequestRepositoryMoq.Object,
                _authorizationServiceMoq.Object,
                _friendServiceMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task AcceptRequest_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.ConfirmRequest(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AcceptRequest_Success()
        {
            var request = FriendsRequestUnitTestConstants.GetCorrectRequest();

            _friendRequestRepositoryMoq.Setup(x => x.GetRequest(It.IsAny<Guid>(), CancellationToken))
                .ReturnsAsync(request);

            var friendsRequestService = GetService();
            await friendsRequestService.ConfirmRequest(Guid.NewGuid(), CancellationToken);

            _friendRequestRepositoryMoq.Verify(x => x.GetRequest(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetRequestAsync_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.GetRequestAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetRequestAsync_Success()
        {
            var id = Guid.NewGuid();
            var request = FriendsRequestUnitTestConstants.GetCorrectRequest();

            _friendRequestRepositoryMoq.Setup(x => x.GetRequest(id, CancellationToken))
                .ReturnsAsync(request);

            var friendsRequestService = GetService();
            var result = await friendsRequestService.GetRequestAsync(id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.GetSendedRequestsAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_BadClaims()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.GetSendedRequestsAsync(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            _authorizationServiceMoq.Setup(x => x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            _friendRequestRepositoryMoq.Setup(x => x.GetSendedRequestsAsync(user.Id, CancellationToken))
                .ReturnsAsync(FriendsRequestUnitTestConstants.GetCorrectRequests());

            var friendsRequestService = GetService();
            var result = await friendsRequestService.GetSendedRequestsAsync(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.GetIncomingRequestsAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_BadClaims()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.GetIncomingRequestsAsync(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            _authorizationServiceMoq.Setup(x => x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            _friendRequestRepositoryMoq.Setup(x => x.GetIncomingRequestsAsync(user.Id, CancellationToken))
                .ReturnsAsync(FriendsRequestUnitTestConstants.GetCorrectRequests());

            var friendsRequestService = GetService();
            var result = await friendsRequestService.GetIncomingRequestsAsync(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddRequestAsync_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.AddRequestAsync(null, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddRequestAsync_BadClaims()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.AddRequestAsync(new ClaimsPrincipal(), "test", CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteRequestAsync_NullInput()
        {
            var friendsRequestService = GetService();

            await ThrowsTask(async () => await friendsRequestService.DeleteRequestAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteRequestAsync_Success()
        {
            _friendRequestRepositoryMoq.Setup(x => x.DeleteRequestAsync(It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var friendsRequestService = GetService();

            await friendsRequestService.DeleteRequestAsync(Guid.NewGuid(), CancellationToken);

            _friendRequestRepositoryMoq.Verify(x => x.DeleteRequestAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.