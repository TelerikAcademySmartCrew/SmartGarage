using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Models.ViewModels
{
    public class UserUpdateViewModel
    {
        public string? UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}
