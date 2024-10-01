using Moq;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class UnitTypeServiceUnitTests : UnitTestsBase
    {
        [Fact]
        public async Task GetUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = new UnitTypeService(
                new Mock<IUnitTypeStore>().Object);

            await ThrowsTask(async () => await unitTypeService.GetUnitTypeAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();

            var unitTypeStoreMoq = new Mock<IUnitTypeStore>();
            unitTypeStoreMoq.Setup(x =>
            x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = new UnitTypeService(
                unitTypeStoreMoq.Object);

            var result = await unitTypeService.GetUnitTypeAsync(unitType.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUnitTypesAsync_Success()
        {
            var unitTypeResponse = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeResponse();

            var unitTypeStoreMoq = new Mock<IUnitTypeStore>();
            unitTypeStoreMoq.Setup(x =>
            x.GetUnitTypesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(unitTypeResponse);

            var unitTypeService = new UnitTypeService(
                unitTypeStoreMoq.Object);

            var result = await unitTypeService.GetUnitTypesAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = new UnitTypeService(
               new Mock<IUnitTypeStore>().Object);

            await ThrowsTask(async () => await unitTypeService.AddUnitTypeAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();
            var addUnitType = UnitTypeServiceUnitTestConstants.GetCorrectAddUnitTypeEntry();

            var unitTypeStoreMoq = new Mock<IUnitTypeStore>();
            unitTypeStoreMoq.Setup(x =>
            x.AddUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = new UnitTypeService(
                unitTypeStoreMoq.Object);

            var result = await unitTypeService.AddUnitTypeAsync(addUnitType, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = new UnitTypeService(
               new Mock<IUnitTypeStore>().Object);

            await ThrowsTask(async () => await unitTypeService.DeleteUnitTypeAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();

            var unitTypeStoreMoq = new Mock<IUnitTypeStore>();
            unitTypeStoreMoq.Setup(x =>
            x.AddUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);
            unitTypeStoreMoq.Setup(x =>
            x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = new UnitTypeService(
               unitTypeStoreMoq.Object);

            await unitTypeService.DeleteUnitTypeAsync(unitType.Id, CancellationToken);

            unitTypeStoreMoq.Verify(x => x.DeleteUnitTypeAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = new UnitTypeService(
               new Mock<IUnitTypeStore>().Object);

            await ThrowsTask(async () => await unitTypeService.UpdateUnitTypeAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_NotFound()
        {
            var unitTypeService = new UnitTypeService(
               new Mock<IUnitTypeStore>().Object);

            await ThrowsTask(async () => await unitTypeService.UpdateUnitTypeAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();
            var updateUnitType = UnitTypeServiceUnitTestConstants.GetCorrectUpdateUnitTypeEntry();

            var unitTypeStoreMoq = new Mock<IUnitTypeStore>();
            unitTypeStoreMoq.Setup(x =>
            x.UpdateUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);
            unitTypeStoreMoq.Setup(x =>
            x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = new UnitTypeService(
                unitTypeStoreMoq.Object);

            var result = await unitTypeService.UpdateUnitTypeAsync(unitType.Id, updateUnitType, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
