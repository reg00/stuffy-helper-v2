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
    public class FriendServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IFriendRepository> _friendRepositoryMoq = new();
        private readonly Mock<IAuthorizationService> _authorizationServiceMoq = new();

        private FriendService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();
            
            return new FriendService(
                _friendRepositoryMoq.Object,
                _authorizationServiceMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task AddFriendAsync_EmptyInput()
        {
            var friendService = GetService();

            await ThrowsTask(async () => await friendService.AddFriendAsync(string.Empty, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddFriendAsync_TryFriendYourself()
        {
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();

            _authorizationServiceMoq.Setup(x =>
            x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(user);

            var friendService = GetService();

            await ThrowsTask(async () => await friendService.AddFriendAsync(user.Id, user.Id, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteFriendAsync_EmptyInput()
        {
            var friendService = GetService();

            await ThrowsTask(async () => await friendService.DeleteFriendAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteFriendAsync_Succes()
        {
            _friendRepositoryMoq.Setup(x => x.DeleteFriendAsync(It.IsAny<Guid>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var friendService = GetService();
            await friendService.DeleteFriendAsync(Guid.NewGuid(), CancellationToken);

            _friendRepositoryMoq.Verify(x => x.DeleteFriendAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task GetFriends_NullInput()
        {
            var friendService = GetService();

            await ThrowsTask(async () => await friendService.GetFriends(null, cancellationToken: CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetFriends_BadClaims()
        {
            var friendService = GetService();
            
            await ThrowsTask(async () => await friendService.GetFriends(new ClaimsPrincipal(), cancellationToken: CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetFriends_Success()
        {
            var user = AuthorizationServiceUnitTestConstants.GetCorrectUserEntry();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectClaims();

            _authorizationServiceMoq.Setup(x => x.GetUserByName(It.IsAny<string>()))
                .ReturnsAsync(user);

            _friendRepositoryMoq.Setup(x => x.GetFriends(user.Id, 20, 0, CancellationToken))
                .ReturnsAsync(FriendServiceUnitTestConstants.GetCorrectAuthResponse());

            var friendService = GetService();
            var result = await friendService.GetFriends(claims, cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.