﻿using Microsoft.AspNetCore.Identity;

using SmartGarage.Data.Models;

namespace SmartGarage.WebAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime JoinDate { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}