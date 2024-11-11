using Moq;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.Tests.UnitTests.Common;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
namespace StuffyHelper.Tests.UnitTests
{
    public class UnitTypeServiceUnitTests : UnitTestsBase
    {
        private readonly Mock<IUnitTypeStore> _unitTypeRepositoryMoq = new();

        private UnitTypeService GetService()
        {
            return new UnitTypeService(_unitTypeRepositoryMoq.Object);
        }
        
        [Fact]
        public async Task GetUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = GetService();

            await ThrowsTask(async () => await unitTypeService.GetUnitTypeAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task GetUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();

            _unitTypeRepositoryMoq.Setup(x => x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = GetService();
            var result = await unitTypeService.GetUnitTypeAsync(unitType.Id, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task GetUnitTypesAsync_Success()
        {
            var unitTypeResponse = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeResponse();

            _unitTypeRepositoryMoq.Setup(x =>
            x.GetUnitTypesAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(unitTypeResponse);

            var unitTypeService = GetService();
            var result = await unitTypeService.GetUnitTypesAsync(
                cancellationToken: CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task AddUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = GetService();

            await ThrowsTask(async () => await unitTypeService.AddUnitTypeAsync(null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task AddUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();
            var addUnitType = UnitTypeServiceUnitTestConstants.GetCorrectAddUnitTypeEntry();

            _unitTypeRepositoryMoq.Setup(x => x.AddUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = GetService();
            var result = await unitTypeService.AddUnitTypeAsync(addUnitType, CancellationToken);

            await Verify(result, VerifySettings);
        }

        [Fact]
        public async Task DeleteUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = GetService();

            await ThrowsTask(async () => await unitTypeService.DeleteUnitTypeAsync(Guid.Empty, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task DeleteUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();

            _unitTypeRepositoryMoq.Setup(x => x.AddUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);
            _unitTypeRepositoryMoq.Setup(x => x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = GetService();
            await unitTypeService.DeleteUnitTypeAsync(unitType.Id, CancellationToken);

            _unitTypeRepositoryMoq.Verify(x => x.DeleteUnitTypeAsync(It.IsAny<Guid>(), CancellationToken), Times.Once());
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_EmptyInput()
        {
            var unitTypeService = GetService();

            await ThrowsTask(async () => await unitTypeService.UpdateUnitTypeAsync(Guid.Empty, null, CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_NotFound()
        {
            var unitTypeService = GetService();

            await ThrowsTask(async () => await unitTypeService.UpdateUnitTypeAsync(Guid.Parse("e9aa0073-5de0-4227-a5f6-4d6c47d5f9e6"), new(), CancellationToken), VerifySettings);
        }

        [Fact]
        public async Task UpdateUnitTypeAsync_Success()
        {
            var unitType = UnitTypeServiceUnitTestConstants.GetCorrectUnitTypeEntry();
            var updateUnitType = UnitTypeServiceUnitTestConstants.GetCorrectUpdateUnitTypeEntry();

            _unitTypeRepositoryMoq.Setup(x => x.UpdateUnitTypeAsync(It.IsAny<UnitTypeEntry>(), CancellationToken))
                .ReturnsAsync(unitType);
            _unitTypeRepositoryMoq.Setup(x => x.GetUnitTypeAsync(unitType.Id, CancellationToken))
                .ReturnsAsync(unitType);

            var unitTypeService = GetService();
            var result = await unitTypeService.UpdateUnitTypeAsync(unitType.Id, updateUnitType, CancellationToken);

            await Verify(result, VerifySettings);
        }
    }
}
