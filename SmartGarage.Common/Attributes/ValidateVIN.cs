﻿using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Attributes
{
    public class ValidateVIN : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? VIN, ValidationContext validationContext)
        {
            if (VIN is string)
            {
                var s = (string)VIN;

                if (s.Length == 17)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
