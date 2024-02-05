using Microsoft.AspNetCore.Identity;
using Moq;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Tests;

[TestFixture]
public class VehicleTests
{
    private Mock<IVehicleRepository> vehicleRepositoryMock;
    private Mock<UserManager<AppUser>> userManagerMock;
    private IVehicleService vehicleService;

    [SetUp]
    public void Setup()
    {
        vehicleRepositoryMock = new Mock<IVehicleRepository>();

        userManagerMock = MockUserManager<AppUser>();

        vehicleService = new VehicleService(vehicleRepositoryMock.Object,
            userManagerMock.Object);
    }

    // [Test]
    // public async Task CreateVehicleAsync_ValidData_ReturnsMappedVehicleResponseDTO()
    // {
    //
    //     var vehicle = new Vehicle
    //     {
    //         ProductionYear = 2005,
    //         LicensePlateNumber = "E4070MK",
    //         VIN = "78467689092643567",
    //     };
    //
    //     var user = new AppUser
    //     {
    //         Id = Guid.NewGuid().ToString(),
    //     };
    //
    //     var expectedMappedVehicleResponseDto = new VehicleResponseDto()
    //     {
    //         CreationYear = 2005,
    //         LicensePlate = "E4070MK",
    //         VIN = "78467689092643567",
    //     };
    //
    //     userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
    //         .ReturnsAsync(user);
    //
    //     vehicleRepositoryMock.Setup(x => x.CreateVehicleAsync(It.IsAny<Data.Models.Vehicle>(), It.IsAny<AppUser>(), It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(new Data.Models.Vehicle());
    //
    //     // Act
    //     var result = await vehicleService.CreateVehicleAsync(vehicle, "test@example.com", cancellationToken: new CancellationToken());
    //
    //     // Assert
    //     Assert.That(result, Is.EqualTo(expectedMappedVehicleResponseDto));
    // }
    
    [Test]
    public async Task GetAllAsync_ValidQueryParameters_ReturnsMappedVehicleResponseDTOList()
    {
        // Arrange
        var vehicleQueryParameters = new VehicleQueryParameters();

        var vehicles = new List<Data.Models.Vehicle>
        {
            new Data.Models.Vehicle { ProductionYear = 2005,
                LicensePlateNumber = "E4070MK",
                VIN = "78467689092643567", },
            new Data.Models.Vehicle { ProductionYear = 2011,
                LicensePlateNumber = "CB1020MK",
                VIN = "78467689092643095",},
        };

        var expectedMappedVehicleResponseDtoList = vehicles
            .Select(vehicle => new Vehicle
            {
                ProductionYear = vehicle.ProductionYear,
                LicensePlateNumber = vehicle.LicensePlateNumber,
                VIN = vehicle.VIN,
            })
            .ToList();

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetAllAsync(vehicleQueryParameters, new CancellationToken()))
            .ReturnsAsync(vehicles);
        

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, userManagerMock.Object);

        // Act
        var result = await vehicleService.GetAllAsync(vehicleQueryParameters, new CancellationToken());

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IList<Vehicle>>());
        Assert.That(result, Has.Count.EqualTo(expectedMappedVehicleResponseDtoList.Count));

    }
    
    // [Test]
    // public async Task GetVehicleByIdAsync_ValidId_ReturnsMappedDTO()
    // {
    //     // Arrange
    //     var vehicleId = Guid.NewGuid();
    //     var vehicleFromRepository = new Vehicle();
    //     var mappedVehicleDto = new VehicleResponseDto(); // TODO: fix this test
    //
    //     vehicleRepositoryMock.Setup(repo => repo.GetVehicleByIdAsync(vehicleId)).ReturnsAsync(vehicleFromRepository);
    //
    //     // Act
    //     var result = await vehicleService.GetVehicleByIdAsync(vehicleId);
    //
    //     // Assert
    //     Assert.That(result, Is.EqualTo(mappedVehicleDto));
    // }
    
    // [Test] // TODO: rewrite this test
    // public async Task GetVehiclesByUserAsync_ValidUserId_ReturnsMappedDTOList()
    // {
    //     // Arrange
    //     var userId = Guid.NewGuid().ToString();
    //     var vehicleQueryParameters = new VehicleQueryParameters();
    //     var vehicle1 = new Data.Models.Vehicle
    //     {
    //         Id = 1,
    //         BrandId = 1,
    //         ModelId = 1,
    //         VIN = "12345678901234567",
    //         ProductionYear = 2020,
    //         LicensePlateNumber = "ABC123",
    //         UserId = userId,
    //         IsDeleted = false
    //     };
    //     var vehiclesFromRepository = new List<Data.Models.Vehicle>{vehicle1};
    //     
    //     var mappedVehicleDto1 = new Vehicle
    //     {
    //         Brand = "BrandName",
    //         Model = "ModelName",
    //         VIN = "12345678901234567",
    //         CreationYear = 2020,
    //         LicensePlate = "ABC123",
    //         Username = "UserName"
    //     };
    //     var mappedVehicleDtos = new List<Vehicle>{mappedVehicleDto1};
    //
    //     vehicleRepositoryMock.Setup(repo => repo.GetVehiclesByUserAsync(userId, vehicleQueryParameters))
    //         .ReturnsAsync(vehiclesFromRepository);
    //
    //     vehicleDtoMapperMock.Setup(mapper => mapper.Map(vehiclesFromRepository)).Returns(mappedVehicleDtos);
    //
    //     // Act
    //     var result = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
    //
    //     // Assert
    //     Assert.That(result, Is.EqualTo(mappedVehicleDtos));
    // }

    private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
        return mgr;
    }
}