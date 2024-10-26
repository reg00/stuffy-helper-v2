using Moq;
using StuffyHelper.Authorization.Core1.Features;
using StuffyHelper.Authorization.Core1.Features.Friend;
using StuffyHelper.Authorization.Core1.Features.Friends;
using StuffyHelper.Tests.UnitTests.Common;
using System.Security.Claims;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class FriendsRequestUnitTests : UnitTestsBase
    {
        [Fact]
        public async Task AcceptRequest_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.AcceptRequest(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AcceptRequest_Success()
        {
            var request = FriendsRequestUnitTestConstants.GetCorrectRequest();

            var requestStoreMoq = new Mock<IFriendsRequestStore>();
            requestStoreMoq.Setup(x =>
            x.GetRequest(It.IsAny<Guid>(), CancellationToken))
                .ReturnsAsync(request);

            var friendsRequestService = new FriendsRequestService(
                requestStoreMoq.Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await friendsRequestService.AcceptRequest(Guid.NewGuid(), CancellationToken);

            requestStoreMoq.Verify(x => x.GetRequest(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetRequestAsync_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.GetRequestAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetRequestAsync_Success()
        {
            var id = Guid.NewGuid();
            var request = FriendsRequestUnitTestConstants.GetCorrectRequest();

            var requestStoreMoq = new Mock<IFriendsRequestStore>();
            requestStoreMoq.Setup(x =>
            x.GetRequest(id, CancellationToken))
                .ReturnsAsync(request);

            var friendsRequestService = new FriendsRequestService(
                requestStoreMoq.Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            var result = await friendsRequestService.GetRequestAsync(id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.GetSendedRequestsAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_BadClaims()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.GetSendedRequestsAsync(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetSendedRequestsAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            var requestStoreMoq = new Mock<IFriendsRequestStore>();
            requestStoreMoq.Setup(x =>
            x.GetSendedRequestsAsync(user.Id, CancellationToken))
                .ReturnsAsync(FriendsRequestUnitTestConstants.GetCorrectRequests());

            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IFriendService>().Object);

            var result = await friendsRequestService.GetSendedRequestsAsync(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.GetIncomingRequestsAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_BadClaims()
        {
            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.GetIncomingRequestsAsync(new ClaimsPrincipal(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetIncomingRequestsAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            var requestStoreMoq = new Mock<IFriendsRequestStore>();
            requestStoreMoq.Setup(x =>
            x.GetIncomingRequestsAsync(user.Id, CancellationToken))
                .ReturnsAsync(FriendsRequestUnitTestConstants.GetCorrectRequests());

            var friendsRequestService = new FriendsRequestService(
                new Mock<IFriendsRequestStore>().Object,
                authorizationServiceMoq.Object,
                new Mock<IFriendService>().Object);

            var result = await friendsRequestService.GetIncomingRequestsAsync(claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddRequestAsync_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
               new Mock<IFriendsRequestStore>().Object,
               new Mock<IAuthorizationService>().Object,
               new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.AddRequestAsync(null, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddRequestAsync_BadClaims()
        {
            var friendsRequestService = new FriendsRequestService(
               new Mock<IFriendsRequestStore>().Object,
               new Mock<IAuthorizationService>().Object,
               new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.AddRequestAsync(new ClaimsPrincipal(), "test", CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteRequestAsync_NullInput()
        {
            var friendsRequestService = new FriendsRequestService(
               new Mock<IFriendsRequestStore>().Object,
               new Mock<IAuthorizationService>().Object,
               new Mock<IFriendService>().Object);

            await ThrowsTask(async () => await friendsRequestService.DeleteRequestAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteRequestAsync_Success()
        {
            var request = FriendsRequestUnitTestConstants.GetCorrectRequest();

            var requestStoreMoq = new Mock<IFriendsRequestStore>();
            requestStoreMoq.Setup(x =>
            x.DeleteRequestAsync(It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var friendsRequestService = new FriendsRequestService(
                requestStoreMoq.Object,
                new Mock<IAuthorizationService>().Object,
                new Mock<IFriendService>().Object);

            await friendsRequestService.DeleteRequestAsync(Guid.NewGuid(), CancellationToken);

            requestStoreMoq.Verify(x => x.DeleteRequestAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.