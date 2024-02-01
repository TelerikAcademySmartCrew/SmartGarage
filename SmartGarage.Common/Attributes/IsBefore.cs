using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IsBefore : ValidationAttribute
    {
		protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
		{
			if (int.TryParse(value?.ToString(), out int year))
			{
				if (year <= DateTime.UtcNow.Year)
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult(ErrorMessage);
		}
	}
}
