using Moq;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class ParticipantServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IParticipantRepository> _participantRepositoryMoq = new();
        private readonly Mock<IAuthorizationClient> _authorizationClientMoq = new();

        private ParticipantService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new ParticipantService(
                _participantRepositoryMoq.Object,
                _authorizationClientMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task GetParticipantAsync_EmptyInput()
        {
            var participantService = GetService();

            await ThrowsTask(async () => await participantService.GetParticipantAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();

            _participantRepositoryMoq.Setup(x => x.GetParticipantAsync(participant.Id, CancellationToken))
                .ReturnsAsync(participant);

            _authorizationClientMoq.Setup(x => x.GetUserById(participant.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = GetService();
            var result = await participantService.GetParticipantAsync(participant.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetParticipantsAsync_Success()
        {
            var participantResponse = ParticipantServiceUnitTestConstants.GetCorrectParticipantResponse();

            _participantRepositoryMoq.Setup(x =>
            x.GetParticipantsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(participantResponse);

            _authorizationClientMoq.Setup(x => x.GetUserById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = GetService();
            var result = await participantService.GetParticipantsAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddParticipantAsync_EmptyInput()
        {
            var participantService = GetService();

            await ThrowsTask(async () => await participantService.AddParticipantAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();
            var addParticipant = ParticipantServiceUnitTestConstants.GetCorrectUpsertParticipantEntry();

            _participantRepositoryMoq.Setup(x => x.AddParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken))
                .ReturnsAsync(participant);

            _authorizationClientMoq.Setup(x => x.GetUserById(participant.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = GetService();
            var result = await participantService.AddParticipantAsync(addParticipant, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_EmptyInput()
        {
            var participantService = GetService();

            await ThrowsTask(async () => await participantService.UpdateParticipantAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_ParticipantNotFound()
        {
            var participantService = GetService();

            await ThrowsTask(async () => await participantService.UpdateParticipantAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateParticipantAsync_Success()
        {
            var participant = ParticipantServiceUnitTestConstants.GetCorrectParticipantEntry();
            var updateParticipant = ParticipantServiceUnitTestConstants.GetCorrectUpsertParticipantEntry();

            _participantRepositoryMoq.Setup(x => x.UpdateParticipantAsync(It.IsAny<ParticipantEntry>(), CancellationToken))
                .ReturnsAsync(participant);
            _participantRepositoryMoq.Setup(x => x.GetParticipantAsync(participant.Id, CancellationToken))
                .ReturnsAsync(participant);

            _authorizationClientMoq.Setup(x => x.GetUserById(participant.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var participantService = GetService();
            var result = await participantService.UpdateParticipantAsync(participant.Id, updateParticipant, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteParticipantAsync_EmptyInput()
        {
            var participantService = GetService();

            await ThrowsTask(async () => await participantService.DeleteParticipantAsync(string.Empty, Guid.Empty, CancellationToken), VerifySettings);
        }
    }
}
