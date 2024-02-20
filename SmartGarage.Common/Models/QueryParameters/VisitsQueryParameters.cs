using SmartGarage.Common.Enumerations;

namespace SmartGarage.Data.Models.QueryParameters;

public class VisitsQueryParameters
{
    public string? Date { get; set; }

    public string? Owner { get; set; }

    public string? VIN { get; set; }

    public string? LicensePlate { get; set; }

    public Status? Status { get; set; }
}
