using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos;

public class UserRegisterRequestDto
{
    [Required]
    public string Email { get; set; }
}