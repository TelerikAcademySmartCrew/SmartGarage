using Microsoft.AspNetCore.Identity;
using Moq;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;

namespace SmartGarage.Tests;

[TestFixture]
public class VehicleTests
{
    private Mock<IVehicleRepository> vehicleRepositoryMock;
    private Mock<IVehicleDTOMapper> vehicleDtoMapperMock;
    private Mock<UserManager<AppUser>> userManagerMock;
    private IVehicleService vehicleService;

    [SetUp]
    public void Setup()
    {
        vehicleRepositoryMock = new Mock<IVehicleRepository>();

        vehicleDtoMapperMock = new Mock<IVehicleDTOMapper>();
        userManagerMock = MockUserManager<AppUser>();

        vehicleService = new VehicleService(vehicleRepositoryMock.Object, vehicleDtoMapperMock.Object,
            userManagerMock.Object);
    }

    [Test]
    public async Task CreateVehicleAsync_ValidData_ReturnsMappedVehicleResponseDTO()
    {

        var vehicleCreateDto = new VehicleCreateDTO
        {
            CreationYear = 2005,
            LicensePlate = "E4070MK",
            VIN = "78467689092643567",
        };

        var user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
        };

        var expectedMappedVehicleResponseDto = new VehicleResponseDTO
        {
            CreationYear = 2005,
            LicensePlate = "E4070MK",
            VIN = "78467689092643567",
        };

        userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        vehicleDtoMapperMock.Setup(x => x.Map(It.IsAny<VehicleCreateDTO>()))
            .Returns(new Vehicle());

        vehicleRepositoryMock.Setup(x => x.CreateVehicleAsync(It.IsAny<Vehicle>(), It.IsAny<AppUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Vehicle());

        vehicleDtoMapperMock.Setup(x => x.Map(It.IsAny<Vehicle>()))
            .Returns(expectedMappedVehicleResponseDto);

        // Act
        var result = await vehicleService.CreateVehicleAsync(vehicleCreateDto, "test@example.com", cancellationToken: new CancellationToken());

        // Assert
        Assert.That(result, Is.EqualTo(expectedMappedVehicleResponseDto));
    }
    
    [Test]
    public async Task GetAllAsync_ValidQueryParameters_ReturnsMappedVehicleResponseDTOList()
    {
        // Arrange
        var vehicleQueryParameters = new VehicleQueryParameters();

        var vehicles = new List<Vehicle>
        {
            new Vehicle { ProductionYear = 2005,
                LicensePlateNumber = "E4070MK",
                VIN = "78467689092643567", },
            new Vehicle { ProductionYear = 2011,
                LicensePlateNumber = "CB1020MK",
                VIN = "78467689092643095",},
        };

        var expectedMappedVehicleResponseDtoList = vehicles
            .Select(vehicle => new VehicleResponseDTO
            {
                CreationYear = vehicle.ProductionYear,
                LicensePlate = vehicle.LicensePlateNumber,
                VIN = vehicle.VIN,
            })
            .ToList();

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.GetAllAsync(vehicleQueryParameters))
            .ReturnsAsync(vehicles);

        var vehicleDtoMapperMock = new Mock<IVehicleDTOMapper>();
        vehicleDtoMapperMock.Setup(x => x.Map(It.IsAny<IList<Vehicle>>()))
            .Returns(expectedMappedVehicleResponseDtoList);

        var vehicleService = new VehicleService(vehicleRepositoryMock.Object, vehicleDtoMapperMock.Object, userManagerMock.Object);

        // Act
        var result = await vehicleService.GetAllAsync(vehicleQueryParameters);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<IList<VehicleResponseDTO>>());
        Assert.That(result, Has.Count.EqualTo(expectedMappedVehicleResponseDtoList.Count));

    }
    
    [Test]
    public async Task GetVehicleByIdAsync_ValidId_ReturnsMappedDTO()
    {
        // Arrange
        const int vehicleId = 1;
        var vehicleFromRepository = new Vehicle();
        var mappedVehicleDto = new VehicleResponseDTO();

        vehicleRepositoryMock.Setup(repo => repo.GetVehicleByIdAsync(vehicleId)).ReturnsAsync(vehicleFromRepository);

        vehicleDtoMapperMock.Setup(mapper => mapper.Map(vehicleFromRepository)).Returns(mappedVehicleDto);

        // Act
        var result = await vehicleService.GetVehicleByIdAsync(vehicleId);

        // Assert
        Assert.That(result, Is.EqualTo(mappedVehicleDto));
    }
    
    [Test]
    public async Task GetVehiclesByUserAsync_ValidUserId_ReturnsMappedDTOList()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var vehicleQueryParameters = new VehicleQueryParameters();
        var vehicle1 = new Vehicle
        {
            Id = 1,
            BrandId = 1,
            ModelId = 1,
            VIN = "12345678901234567",
            ProductionYear = 2020,
            LicensePlateNumber = "ABC123",
            UserId = userId,
            IsDeleted = false
        };
        var vehiclesFromRepository = new List<Vehicle>{vehicle1};
        
        var mappedVehicleDto1 = new VehicleResponseDTO
        {
            Brand = "BrandName",
            Model = "ModelName",
            VIN = "12345678901234567",
            CreationYear = 2020,
            LicensePlate = "ABC123",
            Username = "UserName"
        };
        var mappedVehicleDtos = new List<VehicleResponseDTO>{mappedVehicleDto1};

        vehicleRepositoryMock.Setup(repo => repo.GetVehiclesByUserAsync(userId, vehicleQueryParameters))
            .ReturnsAsync(vehiclesFromRepository);

        vehicleDtoMapperMock.Setup(mapper => mapper.Map(vehiclesFromRepository)).Returns(mappedVehicleDtos);

        // Act
        var result = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters);

        // Assert
        Assert.That(result, Is.EqualTo(mappedVehicleDtos));
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