﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class VehiclesController : BaseEmployeeController
    {
        private readonly IVehicleService vehicleService;
        private readonly IBrandService brandService;
        private readonly IModelService modelService;
        private readonly IVehicleMapper vehicleMapper;
        private readonly ApplicationDbContext applicationDbContext;

        public VehiclesController(IVehicleService vehicleService,
            IBrandService brandService,
            IModelService modelService,
            IVehicleMapper vehicleMapper,
            ApplicationDbContext applicationDbContext)
        {
            this.vehicleService = vehicleService;
            this.brandService = brandService;
            this.modelService = modelService;
            this.vehicleMapper = vehicleMapper;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> ManageVehicles(VehicleQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            InitializeUserName();

            if (queryParameters == null)
                queryParameters = new VehicleQueryParameters();

            var allVehicles = await vehicleService.GetAllAsync(queryParameters, cancellationToken);

            var vehiclesViewModel = vehicleMapper.ToViewModel(allVehicles);

            var manageVehiclesViewModel = new ManageVehiclesViewModel()
            {
                Vehicles = vehiclesViewModel,
                VehicleQueryParameters = queryParameters
            };

            return View(manageVehiclesViewModel);
        }

        [HttpGet]
        public IActionResult AddVehicleBrand()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterVehicle()
        {
            var model = GetBrandsAndModels();

            InitializeUserName();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle(RegisterVehicleViewModel vehileRegisterData, CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError("CustomerEmail", "User not found");
                var model = GetBrandsAndModels();
                return View(model);
            }

            var brandName = vehileRegisterData.RegisterBrand;
            var modelName = vehileRegisterData.RegisterModel;

            bool b_IsBrandId = Guid.TryParse(brandName, out Guid brandId);
            bool b_IsModelId = Guid.TryParse(modelName, out Guid modelId);

            try
            {
                if (b_IsBrandId && b_IsModelId)
                {
                    // Both brand and model exist. If model name exists but is not related to the brand,
                    // be sure to create a new model and link it to the brand. Note : this should be happening,
                    // as there should not be any known vehicle brands and have same named models
                    var b_IsModelForBrand = this.applicationDbContext.VehicleBrands.Any(b => b.Id == brandId && b.Models.Any(m => m.Id == modelId));
                    if (!b_IsModelForBrand)
                    {
                        // Brand exists but model does not. To create model
                        var brand = brandService.GetByIdAsync(brandId);
                        var newModel = new VehicleModel
                        {
                            Name = modelName,
                            Brand = brand.Result,
                        };
                        var createdModel = await modelService.CreateAsync(newModel);
                        modelId = createdModel.Id;
                    }
                }
                else if (!b_IsBrandId && !b_IsModelId)
                {
                    // Brand and model do not exist. To create both
                    var newBrand = new VehicleBrand
                    {
                        Name = brandName,
                    };
                    var newModel = new VehicleModel
                    {
                        Name = modelName,
                    };

                    newBrand.Models.Add(newModel);
                    newModel.Brand = newBrand;

                    // This will also create the brand
                    var createdModel = await modelService.CreateAsync(newModel);

                    brandId = createdModel.BrandId;
                    modelId = createdModel.Id;
                }
                else if (b_IsBrandId && !b_IsModelId)
                {
                    // Brand exists but model does not. To create model
                    var brand = await brandService.GetByIdAsync(brandId);
                    var newModel = new VehicleModel
                    {
                        Name = modelName,
                        Brand = brand,
                    };
                    var createdModel = await modelService.CreateAsync(newModel);
                    modelId = createdModel.Id;
                }

                var vehicle = new Vehicle()
                {
                    BrandId = brandId,
                    ModelId = modelId,
                    ProductionYear = vehileRegisterData.CreationYear,
                    VIN = vehileRegisterData.VIN,
                    LicensePlateNumber = vehileRegisterData.LicensePlate
                };

                _ = await this.vehicleService.CreateVehicleAsync(vehicle, vehileRegisterData.CustomerEmail, cancellationToken);

                vehileRegisterData.RegisterBrand = vehicle.Brand.Name;
                vehileRegisterData.RegisterModel = vehicle.Model.Name;

                InitializeUserName();

                var model = vehicleMapper.VehicleDataToRegisteredVehicleDataViewModel(vehileRegisterData);

                TempData["VehicleRegisteredModel"] = JsonConvert.SerializeObject(model);

                return RedirectToAction("VehicleRegistered", "Vehicles", new { Area = "Employee" });
            }
            catch (EntityNotFoundException)
            {
                ModelState.AddModelError("CustomerEmail", "User not found");
                var model = GetBrandsAndModels();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult VehicleRegistered()
        {
            try
            {
                string? dataString = TempData["VehicleRegisteredModel"] as string;

                var model = JsonConvert.DeserializeObject<RegisterdVehicleInfoViewModel>(dataString!);

                return View(model);
            }
            catch (Exception)
            {
                return View("RegisterVehicle", "Vehicles");
            }
        }

        private RegisterVehicleViewModel GetBrandsAndModels()
        {
            var brands = this.applicationDbContext.VehicleBrands.Select(b => b).Include(b => b.Models).ToList();

            var model = new RegisterVehicleViewModel();
            model.Brands = brands.Select(b => new RegisterVehicleBrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Models = b.Models.Select(x => new RegisterVehicleModelViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
            }).ToList();

            return model;
        }
    }
}
