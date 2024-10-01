using Moq;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Features.Friends;
using StuffyHelper.Tests.UnitTests.Common;
using System.Security.Claims;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class FriendServiceUnitTests : UnitTestsBase
    {
        [Fact]
        public async Task AddFriendAsync_EmptyInput()
        {
            var friendService = new FriendService(
                new Mock<IFriendStore>().Object,
            new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await friendService.AddFriendAsync(string.Empty, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddFriendAsync_TryFriendYourself()
        {
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            var friendService = new FriendService(
                new Mock<IFriendStore>().Object,
                authorizationServiceMoq.Object);

            await ThrowsTask(async () => await friendService.AddFriendAsync(user.Id, user.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteFriendAsync_EmptyInput()
        {
            var friendService = new FriendService(
                new Mock<IFriendStore>().Object,
            new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await friendService.DeleteFriendAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteFriendAsync_Succes()
        {
            var friendStoreMoq = new Mock<IFriendStore>();
            friendStoreMoq.Setup(x =>
            x.DeleteFriendAsync(It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var friendService = new FriendService(
                friendStoreMoq.Object,
                new Mock<IAuthorizationService>().Object);

            await friendService.DeleteFriendAsync(Guid.NewGuid(), CancellationToken);

            friendStoreMoq.Verify(x => x.DeleteFriendAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetFriends_NullInput()
        {
            var friendService = new FriendService(
                new Mock<IFriendStore>().Object,
                new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await friendService.GetFriends(null, cancellationToken: CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetFriends_BadClaims()
        {
            var friendService = new FriendService(
                new Mock<IFriendStore>().Object,
                new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await friendService.GetFriends(new ClaimsPrincipal(), cancellationToken: CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetFriends_Success()
        {
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            var friendshipStoreMoq = new Mock<IFriendStore>();
            friendshipStoreMoq.Setup(x =>
            x.GetFriends(user.Id, 20, 0, CancellationToken))
                .ReturnsAsync(FriendServiceUnitTestConstants.GetCorrectAuthResponse());

            var friendService = new FriendService(
                friendshipStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await friendService.GetFriends(claims, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.