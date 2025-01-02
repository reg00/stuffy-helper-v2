using Moq;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Tests.Common;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
namespace StuffyHelper.Tests.UnitTests
{
    public class EventServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IEventStore> _eventRepositoryMoq = new();
        private readonly Mock<IParticipantService> _participantServiceMoq = new();
        private readonly Mock<IAuthorizationClient> _authorizationClientMoq = new();
        private readonly Mock<IMediaService> _mediaServiceMoq = new();

        private EventService GetService()
        {
            var mapper = CommonTestConstants.GetMapperConfiguration().CreateMapper();

            return new EventService(
                _eventRepositoryMoq.Object,
                _participantServiceMoq.Object,
                _mediaServiceMoq.Object,
                _authorizationClientMoq.Object,
                mapper);
        }
        
        [Fact]
        public async Task GetEventAsync_EmptyInput()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.GetEventAsync(claims, Guid.Empty, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetEventAsync_EventNotFound()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();
            var eventId = Guid.Parse("90c0fde5-5357-4274-9142-8c5eec2ee3b1");
            _eventRepositoryMoq.Setup(x => x.GetEventAsync(It.IsAny<Guid>(), It.IsAny<string>(), CancellationToken))
                .ThrowsAsync(new EntityNotFoundException($"Event with Id '{eventId}' Not Found."));

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.GetEventAsync(claims, eventId, string.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetEventAsync_Success()
        {
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, eventEntry.UserId, CancellationToken))
                .ReturnsAsync(eventEntry);

            _authorizationClientMoq.Setup(x => x.GetUserById(eventEntry.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var eventService = GetService();
            var result = await eventService.GetEventAsync(claims, eventEntry.Id, eventEntry.UserId, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetEventsAsync_Success()
        {
            var eventResult = EventServiceUnitTestConstants.GetCorrectEventsResponse();

            _eventRepositoryMoq.Setup(x =>
            x.GetEventsAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>(),
                It.IsAny<string?>(),
                It.IsAny<bool?>(),
                It.IsAny<bool?>(),
                It.IsAny<Guid?>(),
                It.IsAny<Guid?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(eventResult);

            var eventService = GetService();
            var result = await eventService.GetEventsAsync(cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddEventAsync_NullInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.AddEventAsync(null, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddEventAsync_UserNotFound()
        {
            _authorizationClientMoq.Setup(x =>
                    x.GetUserById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new EntityNotFoundException($"Пользователь с идентификатором отсутствует"));
            
            var eventService = GetService();
            
            await ThrowsTask(async () => await eventService.AddEventAsync(new(), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddEventAsync_Success()
        {
            var addEventEntry = EventServiceUnitTestConstants.GetCorrectAddEventEntry();
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();
            var claims = AuthorizationServiceUnitTestConstants.GetCorrectStuffyClaims();

            _eventRepositoryMoq.Setup(x => x.AddEventAsync(It.IsAny<EventEntry>(), CancellationToken))
                .ReturnsAsync(eventEntry);

            _authorizationClientMoq.Setup(x => x.GetUserById(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(AuthorizationServiceUnitTestConstants.GetCorrectUserEntry());

            var eventService = GetService();
            var result = await eventService.AddEventAsync(addEventEntry, claims, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteEventAsync_EmptyInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeleteEventAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteEventAsync_EventNotFound()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeleteEventAsync(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteEventAsync_EventAlreadyComleted()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeleteEventAsync(eventEntry.Id, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteEventAsync_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();
            eventEntry.IsCompleted = false;

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();
            await eventService.DeleteEventAsync(eventEntry.Id, null, CancellationToken);

            _eventRepositoryMoq.Verify(x => x.DeleteEventAsync(eventEntry.Id, CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdateEventAsync_EmptyInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdateEventAsync(Guid.Empty, null, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateEventAsync_EventNotFound()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdateEventAsync(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), new(), null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateEventAsync_EventAlreadyComleted()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdateEventAsync(eventEntry.Id, new(), null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateEventAsync_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();
            var updateEntry = EventServiceUnitTestConstants.GetCorrectUpdateEventEntry();
            eventEntry.IsCompleted = false;

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);
            _eventRepositoryMoq.Setup(x => x.UpdateEventAsync(It.IsAny<EventEntry>(), CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();
            var result = await eventService.UpdateEventAsync(eventEntry.Id, updateEntry, null, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeletePrimalEventMedia_EmptyInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeletePrimalEventMedia(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePrimalEventMedia_EventNotFound()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeletePrimalEventMedia(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePrimalEventMedia_EventAlreadyComleted()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.DeletePrimalEventMedia(eventEntry.Id, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeletePrimalEventMedia_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();
            eventEntry.IsCompleted = false;

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);
            _eventRepositoryMoq.Setup(x => x.UpdateEventAsync(It.IsAny<EventEntry>(), CancellationToken))
                .ReturnsAsync(eventEntry);

            _mediaServiceMoq.Setup(x =>
            x.GetPrimalEventMedia(eventEntry.Id, CancellationToken))
                .ReturnsAsync((GetMediaEntry)null);

            var eventService = GetService();
            await eventService.DeletePrimalEventMedia(eventEntry.Id, null, CancellationToken);

            _mediaServiceMoq.Verify(x => x.GetPrimalEventMedia(eventEntry.Id, CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdatePrimalEventMediaAsync_NullInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdatePrimalEventMediaAsync(Guid.Empty, null, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePrimalEventMediaAsync_BadFile()
        {
            var file = AvatarServiceUnitTestConstants.GetNotImageAddAvatarEntry().File;

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdatePrimalEventMediaAsync(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), file, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePrimalEventMediaAsync_EventNotFound()
        {
            var file = AvatarServiceUnitTestConstants.GetImageAddAvatarEntry().File;

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdatePrimalEventMediaAsync(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), file, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePrimalEventMediaAsync_EventAlreadyComleted()
        {
            var file = AvatarServiceUnitTestConstants.GetImageAddAvatarEntry().File;
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();

            await ThrowsTask(async () => await eventService.UpdatePrimalEventMediaAsync(eventEntry.Id, file, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdatePrimalEventMediaAsync_Success()
        {
            var file = AvatarServiceUnitTestConstants.GetImageAddAvatarEntry().File;
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();
            eventEntry.IsCompleted = false;

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);
            _eventRepositoryMoq.Setup(x => x.UpdateEventAsync(It.IsAny<EventEntry>(), CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();
            var result = await eventService.UpdatePrimalEventMediaAsync(eventEntry.Id, file, null, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task CompleteEventAsync_EmptyInput()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.CompleteEventAsync(Guid.Empty, null, true, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompleteEventAsync_EventNotFound()
        {
            var eventService = GetService();

            await ThrowsTask(async () => await eventService.CompleteEventAsync(Guid.Parse("b7e55b0e-6bfe-4e76-90b7-5d3073e22216"), null, true, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task CompleteEventAsync_Success()
        {
            var eventEntry = EventServiceUnitTestConstants.GetCorrectEventEntry();

            _eventRepositoryMoq.Setup(x => x.GetEventAsync(eventEntry.Id, null, CancellationToken))
                .ReturnsAsync(eventEntry);
            _eventRepositoryMoq.Setup(x => x.UpdateEventAsync(It.IsAny<EventEntry>(), CancellationToken))
                .ReturnsAsync(eventEntry);

            var eventService = GetService();
            var result = await eventService.CompleteEventAsync(eventEntry.Id, null, true, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}