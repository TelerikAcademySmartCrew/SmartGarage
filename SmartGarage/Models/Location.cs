namespace SmartGarage.Models
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
            this.LocationId = locationId;
            this.Title = title;
            this.Description = description;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

    }
    public class LocationLists
    {
        public IEnumerable<Location> Locations { get; set; }

        public Location ServiceLocation { get; set; }
    }
}
