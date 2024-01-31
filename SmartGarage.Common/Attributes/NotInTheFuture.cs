using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class NotInTheFuture : ValidationAttribute
    {
		public override bool IsValid(object? value)
		{
			if (value is int year)
			{
				return year <= DateTime.UtcNow.Year;
			}

			return false;
		}
	}
}
