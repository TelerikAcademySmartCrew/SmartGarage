using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class VehiclesController : BaseEmployeeController
    {
        private readonly IVehicleService vehicleService;
        private readonly ApplicationDbContext applicationDbContext;

        public VehiclesController(IVehicleService vehicleService, ApplicationDbContext applicationDbContext)
        {
            this.vehicleService = vehicleService;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult ManageVehicles()
        {
            InitializeUserName();
            return View();
        }

        [HttpGet]
        public IActionResult AddVehicleBrand()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddVehicleBrand(RegisterVehicleViewModel brand) // string brandName, string modelName
        {
            var brandName = brand.RegisterBrand;
            var modelName = brand.RegisterModel;

            if (string.IsNullOrEmpty(brandName) || string.IsNullOrEmpty(modelName))
            {
                ModelState.AddModelError("RegisterBrand", "Brand name cannot be empty.");
                ModelState.AddModelError("RegisterModel", "Model name cannot be empty.");
                return View("RegisterVehicle", brand);
            }
            //else if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("RegisterBrand", "Incorrect brand name.");
            //    ModelState.AddModelError("RegisterModel", "Incorrect model name.");
            //    return View("RegisterVehicle", brand);
            //}

            try
            {
                //var modelExists = applicationDbContext.VehicleModels.Any(m => m.Name == modelName);
                var brandExists = applicationDbContext.VehicleBrands.Any(b => b.Name == brandName);
                var bothExists = applicationDbContext.VehicleBrands.Any(b => b.Name == brandName && b.Models.Any(m => m.Name == modelName));

                if (bothExists)
                {
                    throw new DuplicateEntityFoundException($"Brand {brandName} and model {modelName} already exist.");
                }

                if (brandExists)
                {
                    var newBrand = applicationDbContext.VehicleBrands.FirstOrDefault(b => b.Name == brandName);

                    newBrand.Models.Add(new VehicleModel
                    {
                        Name = modelName,
                        Brand = newBrand
                    });

                    // NOTE : check if we need to save changes
                    applicationDbContext.SaveChanges();
                }
                // Brand nor model exist. Create both
                else
                {
                    var newBrand = new VehicleBrand
                    {
                        Name = brandName,
                        Models = new List<VehicleModel>(new VehicleModel[]
                        {
                            new VehicleModel
                            {
                                Name = modelName,
                            }
                        })
                    };
                    applicationDbContext.VehicleBrands.Add(newBrand);
                    applicationDbContext.SaveChanges();
                }

                return RedirectToAction("RegisterVehicle", "Vehicles");
            }
            catch (DuplicateEntityFoundException ex)
            {
                var brands = applicationDbContext.VehicleBrands.Select(b => b).Include(b => b.Models).ToList();

                //var model = new VehicleRegisterViewModel();
                brand.Brands = brands.Select(b => new RegisterVehicleBrandViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Models = b.Models.Select(x => new RegisterVehicleModelViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                }).ToList();

                ModelState.AddModelError("RegisterBrand", ex.Message);
                ModelState.AddModelError("RegisterModel", ex.Message);
                return View("RegisterVehicle", brand);
            }
        }

        [HttpGet]
        public IActionResult RegisterVehicle()
        {
            var brands = applicationDbContext.VehicleBrands.Select(b => b).Include(b => b.Models).ToList();

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

            InitializeUserName();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle(string brandId, string modelId, RegisterVehicleViewModel vehileRegisterData, CancellationToken cancellationToken)
        {

            //return View(vehileRegisterData);

            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("");
            }

            var vehicle = new Vehicle()
            {
                BrandId = Guid.Parse(brandId),
                ModelId = Guid.Parse(modelId),
                ProductionYear = (int)vehileRegisterData.CreationYear,
                VIN = vehileRegisterData.VIN,
                LicensePlateNumber = vehileRegisterData.LicensePlate
            };

            _ = await this.vehicleService.CreateVehicleAsync(vehicle, "user.01@mail.com", cancellationToken);

            InitializeUserName();

            return RedirectToAction("RegisterVehicle");
        }
    }
}
