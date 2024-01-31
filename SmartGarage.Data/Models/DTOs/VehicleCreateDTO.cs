using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Models.DTOs
{
    public class VehicleCreateDTO
    {
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public string VIN { get; set; }

        [Required, Range(1886, int.MaxValue)]
        public int CreationYear { get; set; }

        [Required]
        public string LicensePlate { get; set; }
    }
}
