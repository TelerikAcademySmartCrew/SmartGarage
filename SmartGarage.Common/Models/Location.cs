namespace SmartGarage.Common.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Location(int locationId,
            string title,
            string description,
            double latitude,
            double longitude)
        {
            LocationId = locationId;
            Title = title;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
        }

    }
    public class LocationsList
    {
        public IEnumerable<Location> Locations { get; set; }

        public Location ServiceLocation { get; set; }
    }
}
