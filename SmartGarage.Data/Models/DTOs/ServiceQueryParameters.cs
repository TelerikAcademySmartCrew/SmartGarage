namespace SmartGarage.Data.Models.DTOs
{
	public class ServiceQueryParameters
    {
        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public string? SortByName { get; set; }

        public string? SortByPrice { get; set; }
    }
}
