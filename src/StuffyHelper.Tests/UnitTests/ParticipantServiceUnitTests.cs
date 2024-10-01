using Moq;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class ParticipantServiceUnitTests : UnitTestsBase
    {
        public ParticipantServiceUnitTests() : base()
        {

        }

        [Fact]
        public async Task GetParticipantAsync_EmptyInput()
        {
            var participantService = new ParticipantService(
                new Mock<IParticipantStore>().Object,
                new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await participantService.GetParticipantAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();

            var participantStoreMoq = new Mock<IParticipantStore>();
            participantStoreMoq.Setup(x =>
            x.GetParticipantAsync(participant.Id, CancellationToken))
                .ReturnsAsync(participant);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(participant.UserId))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = new ParticipantService(
                participantStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await participantService.GetParticipantAsync(participant.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetParticipantsAsync_Success()
        {
            var participantResponse = ParticipantServiceUnitTestConstants.GetCorrectParticipantResponse();

            var participantStoreMoq = new Mock<IParticipantStore>();
            participantStoreMoq.Setup(x =>
            x.GetParticipantsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(participantResponse);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = new ParticipantService(
                participantStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await participantService.GetParticipantsAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddParticipantAsync_EmptyInput()
        {
            var participantService = new ParticipantService(
               new Mock<IParticipantStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await participantService.AddParticipantAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();
            var addParticipant = ParticipantServiceUnitTestConstants.GetCorrectUpsertParticipantEntry();

            var participantStoreMoq = new Mock<IParticipantStore>();
            participantStoreMoq.Setup(x =>
            x.AddParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken))
                .ReturnsAsync(participant);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(participant.UserId))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = new ParticipantService(
                participantStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await participantService.AddParticipantAsync(addParticipant, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_EmptyInput()
        {
            var participantService = new ParticipantService(
               new Mock<IParticipantStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await participantService.UpdateParticipantAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_ParticipantNotFound()
        {
            var participantService = new ParticipantService(
               new Mock<IParticipantStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await participantService.UpdateParticipantAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();
            var updateParticipant = ParticipantServiceUnitTestConstants.GetCorrectUpsertParticipantEntry();

            var participantStoreMoq = new Mock<IParticipantStore>();
            participantStoreMoq.Setup(x =>
            x.UpdateParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken))
                .ReturnsAsync(participant);
            participantStoreMoq.Setup(x =>
            x.GetParticipantAsync(participant.Id, CancellationToken))
                .ReturnsAsync(participant);

            var authorizationServiceMoq = new Mock<IAuthorizationService>();
            authorizationServiceMoq.Setup(x =>
            x.GetUserById(participant.UserId))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = new ParticipantService(
                participantStoreMoq.Object,
                authorizationServiceMoq.Object);

            var result = await participantService.UpdateParticipantAsync(participant.Id, updateParticipant, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteParticipantAsync_EmptyInput()
        {
            var participantService = new ParticipantService(
               new Mock<IParticipantStore>().Object,
               new Mock<IAuthorizationService>().Object);

            await ThrowsTask(async () => await participantService.DeleteParticipantAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteParticipantAsync_Success()
        {
            var participantStoreMoq = new Mock<IParticipantStore>();
            participantStoreMoq.Setup(x =>
            x.DeleteParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken))
                .Returns(Task.CompletedTask);

            var participantService = new ParticipantService(
                participantStoreMoq.Object,
                new Mock<IAuthorizationService>().Object);

            await participantService.DeleteParticipantAsync("test", Guid.NewGuid(), CancellationToken);

            participantStoreMoq.Verify(x => x.DeleteParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken), Times.Once());
        }
    }
}
