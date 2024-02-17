using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ValidateProductionYear : ValidationAttribute
    {
		protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
		{
			if (int.TryParse(value?.ToString(), out int year))
			{
				if (year > 1886 && year <= DateTime.UtcNow.Year)
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult(ErrorMessage);
		}
	}
}
