using Moq;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;

namespace SmartGarage.Tests
{
    [TestClass]
    public class RepairActivityTests
    {
        [TestMethod]
        public async Task AddAsync_Should_AddRepairActivity()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();
            var cancellationToken = new CancellationToken();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<RepairActivity>(), cancellationToken))
                .Returns(Task.FromResult(expectedRepairActivity));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            var actualRepairActivity = await service.AddAsync(expectedRepairActivity, cancellationToken);

            // Assert
            Assert.AreEqual(actualRepairActivity.Id, expectedRepairActivity.Id);
        }

        [TestMethod]
        public async Task DeleteAsync_Should_DeleteRepairActivity()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();
            var cancellationToken = new CancellationToken();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.DeleteAsync(expectedRepairActivity.Id, cancellationToken))
                .Returns(Task.FromResult(expectedRepairActivity));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            await service.DeleteAsync(expectedRepairActivity.Id, cancellationToken);

            // Assert
            repositoryMock.Verify(x => x.DeleteAsync(expectedRepairActivity.Id, cancellationToken), Times.Once());
        }

        [TestMethod]
        public async Task GetById_Should_ReturnRepairActivity()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.GetById(expectedRepairActivity.Id))
                .Returns(Task.FromResult(expectedRepairActivity));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            var actualRepairActivity = await service.GetById(expectedRepairActivity.Id);

            // Assert
            Assert.AreEqual(actualRepairActivity.Id, expectedRepairActivity.Id);
        }

        [TestMethod]
        public async Task GetByName_Should_ReturnRepairActivity()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();
            var repairActivities = new List<RepairActivity> { expectedRepairActivity };
            var cancellationToken = new CancellationToken();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.GetByNameAsync(expectedRepairActivity.RepairActivityType.Name, cancellationToken))
                .Returns(Task.FromResult<ICollection<RepairActivity>>(repairActivities));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            var repairActivitiesCollection = await service.GetByNameAsync(expectedRepairActivity.RepairActivityType.Name, cancellationToken);

            // Assert
            Assert.AreEqual(repairActivitiesCollection.Count, 1);
        }

        [TestMethod]
        public async Task GetByPriceRange_Should_ReturnRepairActivitiesWithPriceInRange()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();            
            var repairActivities = new List<RepairActivity> { expectedRepairActivity };
            var cancellationToken = new CancellationToken();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.GetByPriceRangeAsync(1, 20, cancellationToken))
                .Returns(Task.FromResult<ICollection<RepairActivity>>(repairActivities));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            var repairActivitiesCollection = await service.GetByPriceRange(1, 20, cancellationToken);

            // Assert
            Assert.AreEqual(repairActivitiesCollection.Count, 1);
        }

        [TestMethod]
        public async Task GetByVisitId_Should_ReturnRepairActivities()
        {
            // Arrange
            var expectedRepairActivity = TestData.GetTestRepairActivity();
            var visit = TestData.GetTestVisit();

            var repairActivities = new List<RepairActivity> { expectedRepairActivity };
            var cancellationToken = new CancellationToken();
            var repositoryMock = new Mock<IRepairActivityRepository>();

            repositoryMock
                .Setup(x => x.GetByVisitId(visit.Id, cancellationToken))
                .Returns(Task.FromResult<ICollection<RepairActivity>>(repairActivities));

            var service = new RepairActivityService(repositoryMock.Object);

            // Act
            var repairActivitiesCollection = await service.GetByVisitIdAsync(visit.Id, cancellationToken);

            // Assert
            Assert.AreEqual(repairActivitiesCollection.Count, 1);
        }
    }
}
