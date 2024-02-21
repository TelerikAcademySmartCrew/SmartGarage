using Moq;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;

namespace SmartGarage.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public async Task GetAll_Should_ReturnAllModels()
        {
            // Arrange
            var model = TestData.GetTestModel();
            var repositoryMock = new Mock<IModelRepository>();
            var models = new List<VehicleModel> { model };

            repositoryMock
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult<ICollection<VehicleModel>>(models));

            var service = new ModelService(repositoryMock.Object);

            // Act
            var returnedModels = await service.GetAllAsync();

            // Assert
            Assert.AreEqual(1, returnedModels.Count);
        }

        [TestMethod]
        public async Task GetById_Should_ReturnModel()
        {
            var model = TestData.GetTestModel();
            var repositoryMock = new Mock<IModelRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(model.Id))
                .Returns(Task.FromResult(model));

            var service = new ModelService(repositoryMock.Object);

            // Act
            var returnedModel = await service.GetByIdAsync(model.Id);

            // Assert
            Assert.AreEqual(model.Name, returnedModel.Name);
        }

        [TestMethod]
        public async Task GetByName_Should_ReturnModel()
        {
            var model = TestData.GetTestModel();
            var repositoryMock = new Mock<IModelRepository>();

            repositoryMock
                .Setup(x => x.GetByNameAsync(model.Name))
                .Returns(Task.FromResult(model));

            var service = new ModelService(repositoryMock.Object);

            // Act
            var returnedModel = await service.GetByNameAsync(model.Name);

            // Assert
            Assert.AreEqual(model.Name, returnedModel.Name);
        }

        [TestMethod]
        public async Task Create_Should_CreataeModel()
        {
            var model = TestData.GetTestModel();
            var repositoryMock = new Mock<IModelRepository>();

            repositoryMock
                .Setup(x => x.CreateAsync(model))
                .Returns(Task.FromResult(model));

            var service = new ModelService(repositoryMock.Object);

            // Act
            var returnedModel = await service.CreateAsync(model);

            // Assert
            Assert.AreEqual(model.Name, returnedModel.Name);
        }
    }
}
