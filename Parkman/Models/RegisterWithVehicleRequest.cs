using Parkman.Domain.Enums;

namespace Parkman.Models;

public class RegisterWithVehicleRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string LicensePlate { get; set; } = string.Empty;
    public VehicleBrand Brand { get; set; }
    public VehicleType Type { get; set; }
    public VehiclePropulsionType PropulsionType { get; set; }
    public bool Shareable { get; set; }
}
