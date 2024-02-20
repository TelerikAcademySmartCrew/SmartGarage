using Microsoft.AspNetCore.Identity;

using Moq;

using SmartGarage.Services;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;

namespace SmartGarage.Tests;

[TestClass]
public class VehicleTests
{
    private Mock<IVehicleRepository> mockVehicleRepository;
    private Mock<UserManager<AppUser>> mockUserManager;
    private VehicleService vehicleService;

    [TestInitialize]
    public void Setup()
    {
        mockVehicleRepository = new Mock<IVehicleRepository>();
        mockUserManager = MockUserManager<AppUser>();

        vehicleService = new VehicleService(mockVehicleRepository.Object, mockUserManager.Object);
    }
    
    [TestMethod]
    public async Task GetVehicleByVinAsync_Returns_Vehicle()
    {
        // Arrange
        var vin = "VIN123";
        var cancellationToken = new CancellationToken();
        var expectedVehicle = new Vehicle { VIN = vin };

        mockVehicleRepository.Setup(repo => repo.GetVehicleByVinAsync(vin, cancellationToken))
            .ReturnsAsync(expectedVehicle);

        // Act
        var result = await vehicleService.GetVehicleByVinAsync(vin, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(vin, result.VIN);
    }
    
    [TestMethod]
    public async Task GetVehicleByVinAsync_Returns_Null_When_Not_Found()
    {
        // Arrange
        var vin = "NonExistentVIN";
        var cancellationToken = new CancellationToken();

        mockVehicleRepository.Setup(repo => repo.GetVehicleByVinAsync(vin, cancellationToken))!
            .ReturnsAsync((Vehicle)null!);

        // Act
        var result = await vehicleService.GetVehicleByVinAsync(vin, cancellationToken);

        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public async Task CreateVehicleAsync_WithValidInput_ReturnsVehicle()
    {
        // Arrange
        var vehicle = new Vehicle();
        var email = "user@example.com";
        var cancellationToken = new CancellationToken();
        var user = new AppUser { Id = "userId" };

        mockUserManager.Setup(manager => manager.FindByEmailAsync(email))
            .ReturnsAsync(user);

        mockVehicleRepository.Setup(repo => repo.CreateVehicleAsync(vehicle, user, cancellationToken))
            .ReturnsAsync(vehicle);

        // Act
        var result = await vehicleService.CreateVehicleAsync(vehicle, email, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(user.Id, result.UserId);
    }

    [TestMethod]
    [ExpectedException(typeof(EntityNotFoundException))]
    public async Task CreateVehicleAsync_WithInvalidUser_ThrowsException()
    {
        // Arrange
        var vehicle = new Vehicle();
        var email = "nonexistent@example.com";
        var cancellationToken = new CancellationToken();

        mockUserManager.Setup(manager => manager.FindByEmailAsync(email))!
            .ReturnsAsync((AppUser)null!);

        // Act
        await vehicleService.CreateVehicleAsync(vehicle, email, cancellationToken);

    }
    
    [TestMethod]
    public async Task GetAllAsync_ReturnsListOfVehicles()
    {
        // Arrange
        var vehicleQueryParameters = new VehicleQueryParameters();
        var cancellationToken = new CancellationToken();
        var expectedVehicles = new List<Vehicle> { new Vehicle(), new Vehicle() };

        mockVehicleRepository.Setup(repo => repo.GetAllAsync(vehicleQueryParameters, cancellationToken))
            .ReturnsAsync(expectedVehicles);

        // Act
        var result = await vehicleService.GetAllAsync(vehicleQueryParameters, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedVehicles.Count, result.Count);
    }

    [TestMethod]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoVehiclesFound()
    {
        // Arrange
        var vehicleQueryParameters = new VehicleQueryParameters();
        var cancellationToken = new CancellationToken();

        mockVehicleRepository.Setup(repo => repo.GetAllAsync(vehicleQueryParameters, cancellationToken))
            .ReturnsAsync(new List<Vehicle>());

        // Act
        var result = await vehicleService.GetAllAsync(vehicleQueryParameters, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public async Task GetVehicleByIdAsync_ReturnsVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var cancellationToken = default(CancellationToken);
        var expectedVehicle = new Vehicle { Id = vehicleId };

        mockVehicleRepository.Setup(repo => repo.GetVehicleByIdAsync(vehicleId, cancellationToken))
            .ReturnsAsync(expectedVehicle);

        // Act
        var result = await vehicleService.GetVehicleByIdAsync(vehicleId, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(vehicleId, result.Id);
    }

    [TestMethod]
    public async Task GetVehicleByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var cancellationToken = default(CancellationToken);

        mockVehicleRepository.Setup(repo => repo.GetVehicleByIdAsync(vehicleId, cancellationToken))!
            .ReturnsAsync((Vehicle)null!);

        // Act
        var result = await vehicleService.GetVehicleByIdAsync(vehicleId, cancellationToken);

        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public async Task GetVehicleByLicensePlateAsync_ReturnsVehicle()
    {
        // Arrange
        var licensePlate = "ABC123";
        var cancellationToken = new CancellationToken();
        var expectedVehicle = new Vehicle { LicensePlateNumber = licensePlate };

        mockVehicleRepository.Setup(repo => repo.GetVehicleByLicensePlateAsync(licensePlate, cancellationToken))
            .ReturnsAsync(expectedVehicle);

        // Act
        var result = await vehicleService.GetVehicleByLicensePlateAsync(licensePlate, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(licensePlate, result.LicensePlateNumber);
    }

    [TestMethod]
    public async Task GetVehicleByLicensePlateAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var licensePlate = "NonExistentPlate";
        var cancellationToken = new CancellationToken();

        mockVehicleRepository.Setup(repo => repo.GetVehicleByLicensePlateAsync(licensePlate, cancellationToken))!
            .ReturnsAsync((Vehicle)null!);

        // Act
        var result = await vehicleService.GetVehicleByLicensePlateAsync(licensePlate, cancellationToken);

        // Assert
        Assert.IsNull(result);
    }
    
    [TestMethod]
    public async Task GetVehiclesByUserAsync_ReturnsListOfVehicles()
    {
        // Arrange
        var userId = "userId";
        var vehicleQueryParameters = new VehicleQueryParameters();
        var cancellationToken = new CancellationToken();
        var expectedVehicles = new List<Vehicle> { new Vehicle(), new Vehicle() };

        mockVehicleRepository.Setup(repo => repo.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken))
            .ReturnsAsync(expectedVehicles);

        // Act
        var result = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedVehicles.Count, result.Count);
    }

    [TestMethod]
    public async Task GetVehiclesByUserAsync_ReturnsEmptyList_WhenNoVehiclesFound()
    {
        // Arrange
        var userId = "userId";
        var vehicleQueryParameters = new VehicleQueryParameters();
        var cancellationToken = new CancellationToken();

        mockVehicleRepository.Setup(repo => repo.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken))
            .ReturnsAsync(new List<Vehicle>());

        // Act
        var result = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }
    
    [TestMethod]
    public async Task UpdateVehicleAsync_ReturnsUpdatedVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var modelId = Guid.NewGuid();
        var updatedModel = new VehicleModel { Id = modelId, Name = "UpdatedModel" };
        var updatedVehicle = new Vehicle { Id = vehicleId, ModelId = modelId, Model = updatedModel };
        var cancellationToken = default(CancellationToken);
        var expectedVehicle = new Vehicle { Id = vehicleId, ModelId = modelId, Model = updatedModel };

        mockVehicleRepository.Setup(repo => repo.UpdateVehicleAsync(vehicleId, updatedVehicle, cancellationToken))
            .ReturnsAsync(expectedVehicle);

        // Act
        var result = await vehicleService.UpdateVehicleAsync(vehicleId, updatedVehicle, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(vehicleId, result.Id);
        Assert.AreEqual(modelId, result.ModelId);
        Assert.AreEqual(updatedModel.Name, result.Model.Name);
    }
    
    [TestMethod]
    public async Task DeleteVehicleAsync_CallsDeleteVehicleAsyncInRepository()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var cancellationToken = default(CancellationToken);

        // Act
        await vehicleService.DeleteVehicleAsync(vehicleId, cancellationToken);

        // Assert
        mockVehicleRepository.Verify(repo => repo.DeleteVehicleAsync(vehicleId, cancellationToken), Times.Once);
    }
    
    [TestMethod]
    public async Task DeleteVehicleAsync_RemovesVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var cancellationToken = default(CancellationToken);

        mockVehicleRepository.Setup(repo => repo.DeleteVehicleAsync(vehicleId, cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await vehicleService.DeleteVehicleAsync(vehicleId, cancellationToken);

        // Assert
        mockVehicleRepository.Setup(repo => repo.GetVehicleByIdAsync(vehicleId, cancellationToken))!
            .ReturnsAsync((Vehicle)null!);
        var result = await vehicleService.GetVehicleByIdAsync(vehicleId, cancellationToken);
        Assert.IsNull(result);
    }
    
    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);

        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        return mgr;
    }
}