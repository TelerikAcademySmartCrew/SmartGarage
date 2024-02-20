using Moq;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;

namespace SmartGarage.Tests
{
    [TestClass]
    public class RepairActivityTypeTests
    {
        [TestMethod]
        public async Task Create_Should_CreateRepairActivityType()
        {
            // Arrange
            var repairActivityType = TestData.GetTestRepairActivityType();
            var repositoryMock = new Mock<IRepairActivityTypeRepository>();

            repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<RepairActivityType>()))
                .Returns(Task.FromResult(repairActivityType));

            var service = new RepairActivityTypeService(repositoryMock.Object);

            // Act
            var actualRepairActivityType = await service.CreateAsync(repairActivityType);

            // Assert
            Assert.AreEqual(repairActivityType.Name, actualRepairActivityType.Name);
        }

        [TestMethod]
        public async Task Delete_Should_DeleteRepairActivityType()
        {
            // Arrange
            var repairActivityType = TestData.GetTestRepairActivityType();
            var repositoryMock = new Mock<IRepairActivityTypeRepository>();

            repositoryMock
                .Setup(x => x.DeleteAsync(repairActivityType.Name))
                .Returns(Task.FromResult(repairActivityType));

            var service = new RepairActivityTypeService(repositoryMock.Object);

            // Act
            await service.DeleteAsync(repairActivityType.Name);

            // Assert
            repositoryMock.Verify(x => x.DeleteAsync(repairActivityType.Name), Times.Once);
        }

        [TestMethod]
        public async Task GetAll_Should_ReturnAllRepairActivityTypes()
        {
            // Arrange 
            var repairActivityType = TestData.GetTestRepairActivityType();
            var repositoryMock = new Mock<IRepairActivityTypeRepository>();
            var repairActivityTypes = new List<RepairActivityType> { repairActivityType };

            repositoryMock
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult<ICollection<RepairActivityType>>(repairActivityTypes));

            var service = new RepairActivityTypeService(repositoryMock.Object);

            // Act
            var actualRepairActivityTypes = await service.GetAllAsync();

            // Assert
            Assert.AreEqual(1, actualRepairActivityTypes.Count);
        }

        [TestMethod]
        public async Task Update_Should_UpdateRepairActivityType()
        {
            // Arrange
            var repairActivityType = TestData.GetTestRepairActivityType();
            var repositoryMock = new Mock<IRepairActivityTypeRepository>();
            var newName = "Edited name";

            repositoryMock
                .Setup(x => x.UpdateAsync(repairActivityType.Id, newName))
                .ReturnsAsync(new RepairActivityType { Id = repairActivityType.Id, Name = newName});

            var service = new RepairActivityTypeService(repositoryMock.Object);

            // Act
            var updated = await service.UpdateAsync(repairActivityType.Id, newName);

            // Assert
            Assert.AreEqual(updated.Name, newName);
        }
    }
}
