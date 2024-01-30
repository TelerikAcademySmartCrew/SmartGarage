using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Services.Mappers
{
    public class VehicleDTOMapper : IVehicleDTOMapper
    {
        public Vehicle Map(VehicleDTO vehicleCreateDTO)
        {
            var vehicle = new Vehicle()
            {
                BrandId = vehicleCreateDTO.BrandId,
                ModelId = vehicleCreateDTO.ModelId,
                VIN = vehicleCreateDTO.VIN,
                CreationYear = vehicleCreateDTO.CreationYear,
                LicensePlate = vehicleCreateDTO.LicensePlate,
            };
            return vehicle;
        }
    }
}
