﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class VehicleModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

		[Required]
		public Guid BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public VehicleBrand Brand { get; set; } = null!;

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}