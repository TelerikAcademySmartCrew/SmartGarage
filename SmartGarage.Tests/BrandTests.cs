using Moq;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;

namespace SmartGarage.Tests
{
    [TestClass]
    public class BrandTests
    {
        [TestMethod]
        public async Task GetAll_Should_ReturnAllBrands()
        {
            // Arrange
            var brand = TestData.GetTestBrand();
            var repositoryMock = new Mock<IBrandRepository>();
            var brands = new List<VehicleBrand> { brand };

            repositoryMock
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult<ICollection <VehicleBrand>>(brands));

            var service = new BrandService(repositoryMock.Object);

            // Act
            var returnedBrands = await service.GetAllAsync();
            
            // Assert
            Assert.AreEqual(1, returnedBrands.Count);
        }

        [TestMethod]
        public async Task GetById_Should_ReturnBrand()
        {
            // Arrange
            var brand = TestData.GetTestBrand();
            var repositoryMock = new Mock<IBrandRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(brand.Id))
                .Returns(Task.FromResult(brand));

            var service = new BrandService(repositoryMock.Object);

            // Act
            var returned = await service.GetByIdAsync(brand.Id);

            // Assert
            Assert.AreEqual(brand.Name, returned.Name);
        }

        [TestMethod]
        public async Task GetByName_Should_ReturnBrand()
        {
            // Arrange
            var brand = TestData.GetTestBrand();
            var repositoryMock = new Mock<IBrandRepository>();

            repositoryMock
                .Setup(x => x.GetByNameAsync(brand.Name))
                .Returns(Task.FromResult(brand));

            var service = new BrandService(repositoryMock.Object);

            // Act
            var returned = await service.GetByNameAsync(brand.Name);

            // Assert
            Assert.AreEqual(brand.Name, returned.Name);
        }

        [TestMethod]
        public async Task Create_Should_CreateBrand()
        {
            // Arrange
            var brand = TestData.GetTestBrand();
            var repositoryMock = new Mock<IBrandRepository>();

            repositoryMock
                .Setup(x => x.CreateAsync(brand))
                .Returns(Task.FromResult(brand));

            var service = new BrandService(repositoryMock.Object);

            // Act
            var returned = await service.CreateAsync(brand);

            // Assert
            Assert.AreEqual(brand.Name, returned.Name);
        }
    }
}
