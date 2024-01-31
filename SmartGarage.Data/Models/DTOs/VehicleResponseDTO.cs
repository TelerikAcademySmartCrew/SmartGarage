﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Models.DTOs
{
    public class VehicleResponseDTO
    {
        public string? Brand{ get; set; }
        public string? Model { get; set; }
        public string? VIN { get; set; }
        public int CreationYear { get; set; }
        public string? LicensePlate { get; set; }
        public string? Username { get; set; }
    }
}